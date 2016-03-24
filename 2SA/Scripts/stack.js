var stack;
var s_it = 0;

function InitStack() {
    stack = new Array();
}

function Push(item){
    stack[s_it] = item;
    s_it = s_it+1;
}
function Pop(){
    if(s_it > 0)
        return stack[s_it-1];
}

function Remove(guid) {
    var tmp = new Array();
    var itt = 0;
    jQuery.each(stack, function(it, obj) {
        if (obj.toString() != guid) {
            tmp[itt] = this.toString();
            itt = itt + 1;
        }
    });
    s_it = s_it - 1;
    stack = tmp;
}