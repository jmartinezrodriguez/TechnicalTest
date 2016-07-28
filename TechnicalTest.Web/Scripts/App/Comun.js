
App.Comun = {
    AjaxRequest: function (options, callback, complete) {
        $.ajax({
            url: options.url,
            type: options.type ? options.Type : "POST",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            contentType: options.contentType ? options.contentType : "application/json",
            dataType: options.dataType ? options.dataType : "json",
            data: options.parameters ? options.parameters : "",
            success: function (data) {
                if (data !== null) {
                    callback(data);
                }
            }
        });
    },
    Alert: function (mensaje, tipo) {
        $.growl(
            mensaje, { type: tipo, z_index: 9999 });
    },
    ClearForm: function (Element) {
        $("#" + Element).find(':input').each(function () {
            switch (this.type) {
                case 'password':
                case 'select-multiple':
                case 'select-one':
                case 'text':
                case 'textarea':
                    $(this).val('');
                    break;
                case 'checkbox':
                case 'radio':
                    this.checked = false;
            }
        });
    }
};


