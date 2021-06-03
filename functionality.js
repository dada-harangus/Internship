function addInput(event) {
    event.preventDefault();
   
    var isFilled = document.getElementById("fname").value;
    if (isFilled == "") {
        alert("Name must be provieded");
        return false;
    }
    var dateValid = dateValidation();
    if (dateValid == false) {
        alert("Date must be in mm/dd/yyyy form");
        return false;
    }

    var table = document.getElementById("myTable");
    var name = document.getElementById("fname").value;
    var date = document.getElementById("ldate").value;
    var rating = document.getElementById("rating").value;
    // var genre = document.getElementById("genre").value;
    var dvd = document.getElementById("dvd").checked;
    var row = table.insertRow(1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    var cell5 = row.insertCell(4);

    cell1.innerHTML = name;
    cell2.innerHTML = date;
    cell3.innerHTML = getCheckboxvalues().join();;
    cell4.innerHTML = rating;
    cell5.innerHTML = dvd;

    clearForm();
}


function dateValidation() {

    var date = document.getElementById("ldate").value
    var patt = new RegExp("^\\d{1,2}\\/\\d{1,2}\\/\\d{4}$");
    var test = patt.test(date);
    if (test == false) {
        return false;
    }

    var parts = date.split("/");
    var day = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month == 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];


}

function clearForm() {
    document.getElementById("myForm").reset();
}

function chkcontrol(j) {
    var total = 0;
    
    for (var i = 0; i < document.forms.myForm.ckb.length; i++) {
        if (document.forms.myForm.ckb[i].checked) {
            total = total + 1;
        }
        if (total > 3) {
            alert("Please Select only three")
            document.forms.myForm.ckb[j].checked = false;
            return false;
        }
    }
}


function getCheckboxvalues() {
    var values = [];
    var cbs = document.forms['myForm'].elements['ckb'];
    for (var i = 0, cbLen = cbs.length; i < cbLen; i++) {
        if (cbs[i].checked) {
            values.push(cbs[i].value);
        }
    }
    return values;
}