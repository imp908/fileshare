"use strict";
exports.__esModule = true;
var QuestionType = /** @class */ (function () {
    function QuestionType(ID_, type_) {
        this.ID_ = ID_;
        this.type_ = type_;
        this.ID = ID_;
        this.type = type_;
    }
    return QuestionType;
}());
var QuestionExport = /** @class */ (function () {
    function QuestionExport() {
    }
    QuestionExport.prototype.getQuestions = function () {
        var questions = new QuestionType(0, ['a', 'b', 'c']);
        return questions;
    };
    return QuestionExport;
}());
exports.QuestionExport = QuestionExport;
