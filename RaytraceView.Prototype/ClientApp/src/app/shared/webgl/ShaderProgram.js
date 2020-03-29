"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ShaderProgram = /** @class */ (function () {
    function ShaderProgram(gl, shaderList) {
        this.gl = gl;
        this.shaderObjs = [];
        for (var i_sh = 0; i_sh < shaderList.length; ++i_sh) {
            var shderObj = this.compile(shaderList[i_sh].source, shaderList[i_sh].stage);
            if (shderObj)
                this.shaderObjs.push(shderObj);
        }
        this.progObj = this.link(this.shaderObjs);
        if (this.progObj) {
            this.attrInx = {};
            var noOfAttributes = this.gl.getProgramParameter(this.progObj, this.gl.ACTIVE_ATTRIBUTES);
            for (var i_n = 0; i_n < noOfAttributes; ++i_n) {
                var name = this.gl.getActiveAttrib(this.progObj, i_n).name;
                this.attrInx[name] = this.gl.getAttribLocation(this.progObj, name);
            }
            this.uniLoc = {};
            var noOfUniforms = this.gl.getProgramParameter(this.progObj, this.gl.ACTIVE_UNIFORMS);
            for (var i_n = 0; i_n < noOfUniforms; ++i_n) {
                var name = this.gl.getActiveUniform(this.progObj, i_n).name;
                this.uniLoc[name] = this.gl.getUniformLocation(this.progObj, name);
            }
        }
    }
    ShaderProgram.prototype.delete = function () {
        var _this = this;
        this.shaderObjs.forEach(function (shaderObj) { _this.gl.deleteShader(shaderObj); });
        this.gl.deleteProgram(this.progObj);
    };
    ShaderProgram.prototype.attrI = function (name) { return this.attrInx[name]; };
    ShaderProgram.prototype.uniformL = function (name) { return this.uniLoc[name]; };
    ShaderProgram.prototype.use = function () { this.gl.useProgram(this.progObj); };
    ShaderProgram.prototype.setI1 = function (name, val) { if (this.uniLoc[name])
        this.gl.uniform1i(this.uniLoc[name], val); };
    ShaderProgram.prototype.setF1 = function (name, val) { if (this.uniLoc[name])
        this.gl.uniform1f(this.uniLoc[name], val); };
    ShaderProgram.prototype.setF2 = function (name, arr) { if (this.uniLoc[name])
        this.gl.uniform2fv(this.uniLoc[name], arr); };
    ShaderProgram.prototype.setF3 = function (name, arr) { if (this.uniLoc[name])
        this.gl.uniform3fv(this.uniLoc[name], arr); };
    ShaderProgram.prototype.setF4 = function (name, arr) { if (this.uniLoc[name])
        this.gl.uniform4fv(this.uniLoc[name], arr); };
    ShaderProgram.prototype.setM33 = function (name, mat) { if (this.uniLoc[name])
        this.gl.uniformMatrix3fv(this.uniLoc[name], false, mat); };
    ShaderProgram.prototype.setM44 = function (name, mat) { if (this.uniLoc[name])
        this.gl.uniformMatrix4fv(this.uniLoc[name], false, mat); };
    ShaderProgram.prototype.compile = function (source, shaderStage) {
        var shaderScript = document.getElementById(source);
        if (shaderScript)
            source = shaderScript.text;
        var shaderObj = this.gl.createShader(shaderStage);
        this.gl.shaderSource(shaderObj, source);
        this.gl.compileShader(shaderObj);
        var status = this.gl.getShaderParameter(shaderObj, this.gl.COMPILE_STATUS);
        if (!status)
            alert(this.gl.getShaderInfoLog(shaderObj));
        return status ? shaderObj : null;
    };
    ShaderProgram.prototype.link = function (shaderObjs) {
        var prog = this.gl.createProgram();
        for (var i_sh = 0; i_sh < shaderObjs.length; ++i_sh)
            this.gl.attachShader(prog, shaderObjs[i_sh]);
        this.gl.linkProgram(prog);
        status = this.gl.getProgramParameter(prog, this.gl.LINK_STATUS);
        if (!status)
            alert(this.gl.getProgramInfoLog(prog));
        return status ? prog : null;
    };
    return ShaderProgram;
}());
exports.ShaderProgram = ShaderProgram;
//# sourceMappingURL=ShaderProgram.js.map