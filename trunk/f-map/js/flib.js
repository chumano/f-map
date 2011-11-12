//actions is an array : [{"name":attrName1,"value":attrValue1},{"name":attrName2,"value":attrValue2}]
function para2Str(actions) {
    if (actions == '') return '';

    var jSon;
    var myObject = "jSon=" + actions;
    eval(myObject);

    var resultStr = '?';
    for (var i = 0; i < jSon.length; i++) {
        if (i != 0)
            resultStr += '&';

        resultStr += jSon[i].name + '=' + jSon[i].value;
    }

    return resultStr;
}

function str2Arr(str) {
    var jSon;
    var myObject = "jSon=" + str;
    eval(myObject);

    var arr = [];
    for (var i = 0; i < jSon.length; i++) {
        arr.push([i, jSon[i]]);
    }
    return arr;
}

function change2Str(value) {
    return '"' + value + '"';
}