function ClearValidation() {
    var valSummary = $get(_clientvalSumErrorMsg);
    if (valSummary)
        $(valSummary).text('');

    var lblUpdate = $get(_clientlblUpdateSucess);
    if (lblUpdate)
        $(lblUpdate).text('');

    var lblError = $get(_clientlblErrorMsg);
    if (lblError)
        $(lblError).text('');
}

function ValidateUploadedFile(src, args) {

    if ($get(_clientoptFilePath).checked) {
        var fileUpload = $get(_clientFileUploadClientId);
        var oFileName = fileUpload.value;
        var oldFileName = $get(_clienthidoldFileName).value;

        var lblUpdate = $get(_clientlblUpdateSucess);
        if (lblUpdate.innerText != '')
            $get(_clientlblUpdateSucess).innerText = '';
        if ($get(_clientoptFilePath).checked)
            if (oFileName == '' && oldFileName == '') {
                args.IsValid = false;
                src.errormessage = "File to upload should be selected.";
            }
            else if (fileUpload.files && fileUpload.files.length > 0) {
                var file = fileUpload.files[0];

                if (file && file.name) {
                    var fileName = file.name;

                    if (fileName == '') {
                        args.IsValid = false;
                        src.errormessage = 'File name should not be blank.';
                    }

                    if (fileName.indexOf('.') == -1) {
                        args.IsValid = false;
                        src.errormessage = 'Can not upload file without an extension.';
                    }
                    else {
                        var ext = fileName.split('.').pop().toLowerCase();
                        if (!(ext == 'pdf' || ext == 'doc' || ext == 'docx' || ext == 'xls' || ext == 'xlsx' || ext == 'ppt' || ext == 'pptx' || ext == 'pps' || ext == 'ppsx')) {
                            args.IsValid = false;
                            src.errormessage = 'Invalid file type uploaded. Valid extensions are .pdf, .doc, .docx, .xls, .xlsx , .ppt ,.pptx ,.pps and .ppsx';
                        }
                    }
                }

                if (file && file.size > 5242880) {
                    args.IsValid = false;
                    src.errormessage = 'File to upload should not exceed 5mb in size.';
                }
            }
        }
}