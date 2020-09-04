$.validator.addMethod('requiredwhen', function (value, element, params) {
    const targetPropertyName = params[0];
    const targetValues = params[1].split(',');
    const targetValue = getValueByName(targetPropertyName);
    if (targetValues.includes(targetValue)) {
        return value !== "";
    }

    return true;
});

$.validator.unobtrusive.adapters.add('requiredwhen', ['targetPropertyName', 'targetValues'], function (options) {
    options.rules['requiredwhen'] = [options.params['targetPropertyName'], options.params['targetValues']];
    options.messages['requiredwhen'] = options.message;
});

const getValueByName = targetPropertyName => {
    const control = $(`[name=${targetPropertyName}]`);
    switch (control.attr("type")) {
        case "checkbox":
        case "radio":
            return $(`[name=${targetPropertyName}]:checked`).val();
        default:
            return $(`[name=${targetPropertyName}]`).val();
    }
}
