$(function () { 
    $('[data-toggle="tooltip"]').tooltip(); 
})


function onCopy(element) {
    var innerText = element.innerText;
    var input = document.createElement('input');
    input.setAttribute('type', 'hide');
    input.setAttribute('value', innerText);
    document.body.appendChild(input);
    input.select();
    input.setSelectionRange(0, 9999);
    document.execCommand('Copy');
    if (document.execCommand('Copy')) {
        var arg = {
            class: 'bg-success',
            autohide: true,
            delay: 3000,
            body: innerText,
            title: "复制成功"
        };
        $(document).Toasts('create', arg);
    }
    input.remove();
}