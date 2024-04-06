$(function () {
    $('#addimg').change(function() {
        let input = this, file = input.files[0];
        if (file) {
            let reader = new FileReader();
            reader.onload = function(e) {
                $('#previewImage').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    $('#addimg2').change(function () {
        let input = this, file = input.files[0];
        if (file) {
            let reader = new FileReader();
            reader.onload = function (e) {
                $('#previewImage2').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
});