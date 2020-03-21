"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Texture = /** @class */ (function () {
    function Texture(gl, image, texUnit) {
        if (texUnit === void 0) { texUnit = 0; }
        this.gl = gl;
        this.image = image;
        this.texUnit = texUnit;
        this.size = [this.image.naturalWidth, this.image.naturalHeight];
        //let cx = 1 << 31 - Math.clz32(this.image.naturalWidth);
        //if (cx < this.image.naturalWidth) cx *= 2;
        //let cy = 1 << 31 - Math.clz32(this.image.naturalHeight);
        //if (cy < this.image.naturalHeight) cy *= 2;
        //var canvas = document.createElement('canvas');
        //canvas.width = cx;
        //canvas.height = cy;
        //var context = canvas.getContext('2d');
        //context.drawImage(this.image, 0, 0, canvas.width, canvas.height);
        this.textureObj = this.createTexture2D(this.image, true);
        //this.size = [cx, cy];
    }
    Texture.prototype.delete = function () {
        this.gl.deleteTexture(this.textureObj);
    };
    Texture.prototype.createTexture2D = function (image, flipY) {
        var t = this.gl.createTexture();
        this.gl.activeTexture(this.gl.TEXTURE0 + this.texUnit);
        this.gl.bindTexture(this.gl.TEXTURE_2D, t);
        this.gl.pixelStorei(this.gl.UNPACK_FLIP_Y_WEBGL, flipY != undefined && flipY == true);
        if (image.cx && image.cy && image.plane)
            this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, image.cx, image.cy, 0, this.gl.RGBA, this.gl.UNSIGNED_BYTE, new Uint8Array(image.plane));
        else
            this.gl.texImage2D(this.gl.TEXTURE_2D, 0, this.gl.RGBA, this.gl.RGBA, this.gl.UNSIGNED_BYTE, image);
        this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MAG_FILTER, this.gl.LINEAR);
        this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_MIN_FILTER, this.gl.LINEAR);
        this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_S, this.gl.REPEAT);
        this.gl.texParameteri(this.gl.TEXTURE_2D, this.gl.TEXTURE_WRAP_T, this.gl.REPEAT);
        this.gl.bindTexture(this.gl.TEXTURE_2D, null);
        this.gl.activeTexture(this.gl.TEXTURE0);
        return t;
    };
    Texture.prototype.bind = function (texUnit) {
        var unit = texUnit || this.texUnit;
        this.gl.activeTexture(this.gl.TEXTURE0 + unit);
        if (this.textureObj) {
            this.gl.bindTexture(this.gl.TEXTURE_2D, this.textureObj);
            return true;
        }
        this.gl.bindTexture(this.gl.TEXTURE_2D, this.dummyObj);
        return false;
    };
    Texture.prototype.bindDflt = function (texUnit) {
        this.gl.activeTexture(this.gl.TEXTURE0 + texUnit);
        this.gl.bindTexture(this.gl.TEXTURE_2D, this.dummyObj);
        return false;
    };
    return Texture;
}());
exports.Texture = Texture;
;
//# sourceMappingURL=Texture.js.map