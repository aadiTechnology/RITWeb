CKEDITOR.editorConfig = function (config) {
    config.scayt_autoStartup = true;
    config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'paragraph', groups: ['align', 'blocks', 'indent', 'list', 'bidi', 'paragraph'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		'/',
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		'/',
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
	];
    config.enterMode = CKEDITOR.ENTER_BR;    
    config.removeButtons = 'Print,Templates,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Blockquote,CreateDiv,BidiLtr,BidiRtl,Language,Anchor,SpecialChar,PageBreak,Iframe,ShowBlocks,About,Source,Save,Flash,Preview';    
};