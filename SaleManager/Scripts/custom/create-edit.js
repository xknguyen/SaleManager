$(document).ready(function () {
    tinymce.init({
        selector: "textarea#Description",
        entity_encoding: "raw",
        theme: "modern",
        height: 200,
        menubar: false,
        plugins: [
             "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
             "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
             "save table contextmenu directionality emoticons template paste textcolor"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link image | forecolor backcolor",
        style_formats: [
             { title: 'Bold text', inline: 'b' },
             { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
             { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
             { title: 'Table styles' },
             { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ]
    });
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green'
    });
});