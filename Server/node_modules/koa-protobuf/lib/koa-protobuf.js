const parse = require('co-body');
const contentType = require('content-type');
const getRawBody = require('raw-body');

/**
 * Parse a protobuf message sent by a client.
 *
 * This will not execute further middleware if no message was found
 * or if it was not valid wire format.
 *
 * The message is stored in `ctx.request.proto`.
 *
 * The `parseJson` option is enabled by default, this allows for
 * protobuf-compliant JSON to be accepted and then turned into a
 * valid protobuf.js messsage object.
 *
 * @param {Object} messageType - protobuf.js message type
 * @param {Object} [options]
 * @param {boolean} [options.parseJson=true]
 */
module.exports.protobufParser = function (messageType, options = {}) {
  return async function (ctx, next) {
    let type = contentType.parse(ctx).type;
    let success;

    if (type === 'application/x-protobuf') {
      success = await _parseProtobuf(ctx, messageType);
    } else if (type === 'application/json' && options.parseJson !== false) {
      success = await _parseJson(ctx, messageType);
    } else {
      success = false;

      let message = options.parseJson !== false ?
          'Only content types `application/x-protobuf` and ' +
          '`application/json` are allowed for message ' + messageType.name :
          'Only content type `application/x-protobuf` is allowed for ' +
          'message ' + messageType.name;

      ctx.throw(415, message);
    }

    // We'll only execute more middleware if the parsing was
    // successful.
    if (success) {
      await next();
    }
  };
}

/**
 * Send a protobuf message to the client.
 *
 * Encodes a message stored in `ctx.proto` or `ctx.response.proto`
 * into the protobuf wire format.
 *
 * The `sendJson` option is enabled by default, this allows for
 * protobuf-compliant JSON to be send to the client if requested.
 *
 * @param {Object} [options]
 * @param {boolean} [options.sendJson=true]
 */
module.exports.protobufSender = function (options = {}) {
  return async function (ctx, next) {
    // Execute other middleware first.
    await next();

    let proto = ctx.proto || ctx.response.proto;

    if (proto !== undefined) {
      let success;

      // If the client accepts both types, protobuf wire encoding is
      // prefered.
      switch (ctx.accepts('application/x-protobuf', 'application/json')) {
        case 'application/x-protobuf':
          _encodeProtobuf(ctx, proto);
          success = true;
          break;
        case 'application/json':
          // Only encoding JSON if the options allow for it, if not,
          // then we'll mark this as unsuccessful and return a 406
          // down below.
          if (options.sendJson !== false) {
            _encodeJson(ctx, proto);
            success = true;
          } else {
            success = false;
          }
          break;
        default:
          success = false;
      }

      if (!success) {
        let message = options.sendJson !== false ?
          'Only `application/x-protobuf` and `application/json` can be given' +
          'for message ' + message.constructor.name :
          'Only `application/x-protobuf` can be given for message ' +
          message.constructor.name;

        ctx.throw(406, message);
      }
    }
  }
}

async function _parseProtobuf(ctx, messageType) {
  try {
    let body = await getRawBody(ctx.req, { length: ctx.request.length });
    ctx.request.proto = messageType.decode(body);
    return true;
  } catch (err) {
    ctx.throw(415, `Invalid wire format for message ${messageType.name}`);
    return false;
  }
}

async function _parseJson(ctx, messageType) {
  let obj;

  // If bodyparser is registered and parsed the JSON beforehand, then
  // we'll go ahead and use the parsed JSON instead of doing it
  // manually.
  if (ctx.request.body !== undefined) {
    obj = ctx.request.body;
  } else {
    try {
      obj = await parse.json(ctx.req);
    } catch (err) {
      ctx.throw(415, 'Invalid JSON');
      return false;
    }
  }

  let err = messageType.verify(obj);

  if (err) {
    ctx.throw(415, `JSON was not compatible with message ${messageType.name}`);
    return false;
  } else {
    ctx.request.proto = messageType.fromObject(obj);
    return true;
  }
}

function _encodeProtobuf(ctx, proto) {
  ctx.type = 'application/x-protobuf';
  ctx.body = proto.constructor.encode(proto).finish();
}

function _encodeJson(ctx, proto) {
  ctx.type = 'application/json';
  ctx.body = proto.toJSON();
}
