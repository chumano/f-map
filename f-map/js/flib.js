// convert an array of parameters to string
// actions is an array : [{"name":attrName1,"value":attrValue1},{"name":attrName2,"value":attrValue2}]
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

// convert a string in JSON format to array
function str2Arr(str) {
    var jSon;
    var myObject = "jSon=" + str;
    eval(myObject);

    var str = '';
    var arr = [];
    for (var i = 0; i < jSon.length; i++) {
        arr.push([i, jSon[i]]);
        
        if (i != 0) str += '<br/>';
        str +='<i>'+ jSon[i]+ '</i>';
    }

    wardsWin.body.update(str);
    wardsWin.show();
    return arr;
}

function change2Str(value) {
    return '"' + value + '"';
}
