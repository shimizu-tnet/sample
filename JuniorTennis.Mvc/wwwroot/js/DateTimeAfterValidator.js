$.validator.addMethod('datetimeafter', function (value, element, params) {
    const beginId = params[0];
    const allowEquivalent = params[1];
    const beginValue = document.getElementById(beginId).value;
    if (beginValue === "") {
        return true;
    }

    if (value === "") {
        return true;
    }

    const beginDatetime = new Date(beginValue).getTime();
    const endDateTime = new Date(value).getTime();
    if (allowEquivalent === 'true') {
        if (beginDatetime === endDateTime) {
            return true;
        }
    }

    return beginDatetime < endDateTime;
});

$.validator.unobtrusive.adapters.add('datetimeafter', ['beginProperty', 'allowEquivalent'], function (options) {
    options.rules['datetimeafter'] = [options.params['beginProperty'], options.params['allowEquivalent']];
    options.messages['datetimeafter'] = options.message;
});
