function Movie(id, name, date, genre, rating, dvd) {
    this.id = id,
        this.name = name;
    this.date = date;
    this.genre = genre;
    this.rating = rating;
    this.dvd = dvd;
}
var movieList = [];

function GetDataFromStorage() {
    var submitButton = document.getElementById('submit');
    submitButton.style.display = 'inline';

    var submitButton = document.getElementById('submitEdit');
    submitButton.style.display = 'none';
    var table = document.getElementById("myTable");
    promise = $.ajax({
        url: "https://localhost:44370/api/movies/GetAllMovies",
        method: "GET",
        data: { format: 'json' },
        
    });
    promise.then(function (data) {
        console.log(data);
        movieList = data;
        clearForm();
        clearTable();
        for (var i = 0; i < data.length; i++)
            InsertDataIntoTable(i, table, data[i]);

    });
}

function addInput(event, movieListIndex) {
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

    var divStar = document.getElementsByClassName("fa fa-star");
    var contor = 0;
    for (var i = 0; i < divStar.length; i++) {

        if (divStar[i].className == 'fa fa-star fa-star-selected') {
            contor = divStar[i].getAttribute('id');
        }
    }

    var promise;
    if (movieListIndex != undefined) {
        promise = $.ajax({
            method: "PUT",
            url: "https://localhost:44370/api/movies/UpdateMovie",
            data: JSON.stringify({
                movieId: movieList[movieListIndex].movieId,
                name: document.getElementById("fname").value,
                releaseDate: new Date(document.getElementById("ldate").value),
                rating: contor,
                releasedOnDvd: document.getElementById("dvd").checked,
                genreList: getCheckboxvalues()
            }),
            contentType: "application/json"
        });
    } else {
        promise = $.ajax({
            method: "POST",
            url: "https://localhost:44370/api/movies/SaveMovie",
            data: JSON.stringify({
                name: document.getElementById("fname").value,
                releaseDate: new Date(document.getElementById("ldate").value),
                rating: 0,
                releasedOnDvd: document.getElementById("dvd").checked,
                genreList: getCheckboxvalues()
            }),
            contentType: "application/json"
        });
    }

    promise.then(function () {
        GetDataFromStorage();
    }, function (error) {
        if (error.responseJSON.errors["Rating"] != undefined){
            alert(error.responseJSON.errors["Rating"]);
        }
       
       
    });
}

function dateValidation() {

    var date = document.getElementById("ldate").value
    var patt = new RegExp("^\\d{1,2}\\/\\d{1,2}\\/\\d{4}$");
    var test = patt.test(date);
    if (test == false) {
        return false;
    }

    var parts = date.split("/");
    var day = parseInt(parts[1], 10);
    var month = parseInt(parts[0], 10);
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

function InsertDataIntoTable(contor, table, movie) {

    var row = table.insertRow(contor + 1);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    var cell5 = row.insertCell(4);
    var cell6 = row.insertCell(5);
    var cell7 = row.insertCell(6);

    cell1.innerHTML = movie ? movie.name : movieList[contor].name;
    cell2.innerHTML = movie ? movie.releaseDate : movieList[contor].date;
   //aici o sa pice la sortare 
    for (var i = 0; i < movie.genreList.length; i++) {
        cell3.innerHTML += ' ' + movie.genreList[i].genreName.toString();
    }

    cell4.innerHTML = movie ? movie.rating : movieList[contor].rating;
    cell5.innerHTML = movie ? movie.releasedOnDvd : movieList[contor].dvd;

    var buttonDelete = document.createElement('button');
    buttonDelete.innerHTML = "Delete";
    cell6.appendChild(buttonDelete);
    buttonDelete.setAttribute('onclick', '(DeleteRow(' + (contor) + '))');

    var buttonEdit = document.createElement('button');
    buttonEdit.innerHTML = "Edit";
    cell7.appendChild(buttonEdit);
    buttonEdit.setAttribute('onclick', '(EditRow(' + (contor) + '))');
    row.setAttribute('ondblclick', '(EditRow(' + contor + '))');
    buttonDelete.classList.add('button');
    buttonEdit.classList.add('button');


}


function DeleteRow(movieListIndex) {
    promise = $.ajax({
        method: "DELETE",
        url: "https://localhost:44370/api/movies/DeleteMovie?movieId=" + movieList[movieListIndex].movieId,

        contentType: "application/json"

    });
    promise.then(function () {
        clearTable();
        GetDataFromStorage()
    });
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

function SortDirection(columnIndex) {

    var spanAsc = document.getElementsByClassName("asc");
    var spanDesc = document.getElementsByClassName("desc");
    if (getComputedStyle(spanAsc[columnIndex]).display == 'none') {

        for (var i = 0; i < spanAsc.length; i++) {

            spanAsc[i].style.display = "none";
            spanDesc[i].style.display = "none";
        }
        spanAsc[columnIndex].style.display = "inline";
        return 'asc';
    } else {

        for (var i = 0; i < spanDesc.length; i++) {

            spanDesc[i].style.display = "none";
            spanAsc[i].style.display = "none";

        }
        spanDesc[columnIndex].style.display = "inline";
        return 'desc';
    }
}


function SortTable(t) {

    let ColumnValue = new Map();
    ColumnValue[0] = 'name';
    ColumnValue[1] = 'date';
    ColumnValue[2] = 'genre'
    ColumnValue[3] = 'rating';
    ColumnValue[4] = 'dvd';

    movieList.sort(compareValues(ColumnValue[t.cellIndex], SortDirection(t.cellIndex)))

    var table = document.getElementById("myTable");
    clearTable();
    for (var i = 0; i < movieList.length; i++)
        InsertDataIntoTable(i, table);
}

function compareValues(key, order = 'asc') {
    return function innerSort(a, b) {
        if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
            // property doesn't exist on either object
            return 0;
        }

        var varA = (typeof a[key] === 'string')
            ? a[key].toUpperCase() : a[key];
        var varB = (typeof b[key] === 'string')
            ? b[key].toUpperCase() : b[key];
        if (key == 'date') {
            varA = new Date(a[key]);
            varB = new Date(b[key]);

        }
        let comparison = 0;
        if (varA > varB) {
            comparison = 1;
        } else if (varA < varB) {
            comparison = -1;
        }
        return (
            (order === 'desc') ? (comparison * -1) : comparison
        );
    };
}
function clearTable() {
    var table = document.getElementById("myTable");
    for (var i = table.rows.length - 1; i > 0; i--) {
        table.deleteRow(i);
    }
}

function selectStar(t) {


    var divStar = document.getElementsByClassName("fa fa-star");

    var index = t.getAttribute('name');

    for (var i = 0; i < divStar.length; i++) {
        divStar[i].classList.remove('fa-star-selected');
    }
    divStar[index - 1].classList.add('fa-star-selected');

}

var editButtonHandler;

function EditRow(movieListIndex) {
    var submitButton = document.getElementById('submit');
    submitButton.style.display = 'none';

    var name = document.getElementById('fname');
    var date = document.getElementById('ldate');
    name.value = movieList[movieListIndex].name;
    var dateFormat = movieList[movieListIndex].releaseDate.split('-')
    date.value = dateFormat[1] + '/' + dateFormat[2].substring(0, 2) + "/" + dateFormat[0];

    if (movieList[movieListIndex].genreList.length > 0) {
        var genreArray = movieList[movieListIndex].genreList;
        var cbs = document.forms['myForm'].elements['ckb'];

        for (var i = 0; i < cbs.length; i++) {
            for (var y = 0; y < genreArray.length; y++) {

                if (cbs[i].value == genreArray[y].genreName.toLowerCase()) {
                    cbs[i].checked = true;
                }

            }
        }
    }
    var divStar = document.getElementsByClassName("fa fa-star");

    divStar[(parseInt(movieList[movieListIndex].rating) - 5) * -1].classList.add('fa-star-selected');
    var checkBoxDvd = document.getElementById('dvd');
    if (movieList[movieListIndex].dvd == true) {
        checkBoxDvd.checked = true;
    }

    var cancelButton = document.getElementById('cancel');
    cancelButton.style.display = 'inline';

    var submitButton = document.getElementById('submitEdit');
    submitButton.style.display = 'inline';

    if (editButtonHandler) {
        submitButton.removeEventListener("click", editButtonHandler);
    }

    editButtonHandler = function (event) {
        addInput(event, movieListIndex);
    };
    submitButton.addEventListener('click', editButtonHandler);
}