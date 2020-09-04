$.validator.addMethod('dateconsistency', function (value, element, params) {
    const yearPropertyId = params[0];
    const yearValue = document.getElementById(yearPropertyId).value;
    if (yearValue === "") {
        return true;
    }

    const monthPropertyId = params[1];
    const monthValue = document.getElementById(monthPropertyId).value;
    if (monthValue === "") {
        return true;
    }

    if (value === "") {
        return ture;
    }

    const verificationDay = new Date(yearValue, monthValue - 1, value);
    var verificationMonth = verificationDay.getMonth() + 1;
    if (verificationMonth.toString() !== monthValue) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.add('dateconsistency', ['yearPropatyName', 'monthPropatyName'], function (options) {
    options.rules['dateconsistency'] = [options.params['yearPropatyName'], options.params['monthPropatyName']];
    options.messages['dateconsistency'] = options.message;
});
