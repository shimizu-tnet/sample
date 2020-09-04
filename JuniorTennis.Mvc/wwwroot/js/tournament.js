const setWithDrawClass = ele => {
    ele.classList.remove("only-point");
    ele.classList.add("with-draw");
}
const setOnlyPointClass = ele => {
    ele.classList.remove("with-draw");
    ele.classList.add("only-point");
}
const showWithDrawMenu = () => [...document.querySelectorAll(".with-draw-menu")].forEach(setWithDrawClass);
const showPointOnlyMenu = () => [...document.querySelectorAll(".with-draw-menu")].forEach(setOnlyPointClass);
const handleRegistrationYearsChanged = async (e) => {
    const registrationYear = `${e.target.value}`.split('/')[0];
    const res = await fetch(`/Tournaments/${registrationYear}/aggregation_months`);
    const json = await res.json();

    const aggregationMonths = document.getElementById("SelectedAggregationMonth");
    while (aggregationMonths.firstChild) aggregationMonths.removeChild(aggregationMonths.firstChild);

    [...json]
        .map(o => {
            const option = document.createElement("option");
            option.value = o.value;
            option.textContent = o.text;
            return option;
        })
        .forEach(o => {
            aggregationMonths.appendChild(o);
        });
}

const isBlank = value => `${value}`.trim().length === 0;
const handleHoldingDateChanged = async () => {
    const holdingStartDate = document.getElementById("HoldingStartDate").value;
    const holdingEndDate = document.getElementById("HoldingEndDate").value;
    if (isBlank(holdingStartDate) || isBlank(holdingEndDate)) {
        return;
    }

    const res = await fetch(`/Tournaments/${holdingStartDate}/${holdingEndDate}/holding_dates`);
    const json = await res.json();

    const holdingDatesTable = document.getElementById("holding-dates-table");
    const holdingDatesTbody = holdingDatesTable.querySelector("tbody");
    while (holdingDatesTbody.firstChild) holdingDatesTbody.removeChild(holdingDatesTbody.firstChild);

    let dataRow;
    for (let i = 0, data = [...json]; i < data.length; i++) {
        if (i % 7 == 0) {
            dataRow = document.createElement("tr");
            holdingDatesTbody.appendChild(dataRow);
        }

        const checkbox = document.createElement("input");
        checkbox.type = "checkbox";
        checkbox.value = data[i].value;
        checkbox.disabled = data[i].disabled;
        checkbox.name = "SelectedHoldingDates";

        const span = document.createElement("span");
        span.innerHTML = data[i].text;

        const label = document.createElement("label");
        label.appendChild(checkbox);
        label.appendChild(span);

        const dataCell = document.createElement("td");
        dataCell.appendChild(label);
        dataRow.appendChild(dataCell);
    }
}

const setPayClass = ele => {
    ele.classList.remove("no-pay");
    ele.classList.add("pay");
}
const setNoPayClass = ele => {
    ele.classList.remove("pay");
    ele.classList.add("no-pay");
}
const methodOfPayments = {
    prePayment: "1",
    postPayment: "2",
    notRecieve: "3",
    other: "4",
}
const visibleEntryFee = () => {
    [...document.querySelectorAll(".entry-fee")].forEach(setPayClass);
    [...document.querySelectorAll(".payment-method-other")].forEach(setNoPayClass);
}
const hideEntryFee = () => {
    [...document.querySelectorAll(".entry-fee")].forEach(setNoPayClass);
    [...document.querySelectorAll(".payment-method-other")].forEach(setPayClass);
}
const showEntryFee = () => {
    const methodOfPayment = `${document.querySelector("[name=SelectedMethodOfPayments]:checked").value}`;
    return methodOfPayment === methodOfPayments.prePayment || methodOfPayment === methodOfPayments.postPayment;
}
const handleMethodOfPaymentChanged = () => {
    if (showEntryFee()) {
        visibleEntryFee();
    } else {
        hideEntryFee();
    }
}

document.getElementById("tournament-type1").addEventListener("click", showWithDrawMenu, false);
document.getElementById("tournament-type2").addEventListener("click", showPointOnlyMenu, false);
document.getElementById("SelectedRegistrationYear").addEventListener("change", handleRegistrationYearsChanged, false);
document.getElementById("HoldingStartDate").addEventListener("change", handleHoldingDateChanged, false);
document.getElementById("HoldingEndDate").addEventListener("change", handleHoldingDateChanged, false);
document.getElementById("method-of-payment1").addEventListener("change", handleMethodOfPaymentChanged, false);
document.getElementById("method-of-payment2").addEventListener("change", handleMethodOfPaymentChanged, false);
document.getElementById("method-of-payment3").addEventListener("change", handleMethodOfPaymentChanged, false);
document.getElementById("method-of-payment4").addEventListener("change", handleMethodOfPaymentChanged, false);

(async () => {
    handleMethodOfPaymentChanged();
})();
