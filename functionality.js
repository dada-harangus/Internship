function Movie(id,name,date,rating, genre,dvd) {
    this.id = id,
    this.name = name;
    this.date = date;
    this.rating = rating;
    this.genre = genre;
    this.dvd = dvd;
  }
 var movieList =[];
 

// document.addEventListener('DOMContentLoaded', GetDataFromStorage(), false);
 
 function GetDataFromStorage(){
 
 if (localStorage.getItem('myDataKey') !== null) {
    movieList = JSON.parse(localStorage.getItem('myDataKey'));
    var table = document.getElementById("myTable");
    for(var i = 0 ; i < movieList.length; i++)
    InsertDataIntoTable(i,table);
  } 
}

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
    var name = document.getElementById("fname").value;
    var date = document.getElementById("ldate").value;
    var rating = document.getElementById("rating").value;
    var genre =  getCheckboxvalues().join();
    var dvd = document.getElementById("dvd").checked;


    var movie = new Movie (id,name, date, genre, rating, dvd);

    while(movie.id == undefined){
        var id =Math.floor(Math.random() * 100);

        if(movieList.find(movie => movie.id == id) == undefined){
            movie.id = id;
            var table = document.getElementById("myTable");
            InsertDataIntoTable(movieList.length,table,movie);
            movieList.push(movie);
        }
    }
    
   localStorage.setItem('myDataKey', JSON.stringify( movieList));
    clearForm();
}



function InsertDataIntoTable(contor,table ,movie){
    
       var row = table.insertRow(contor+1);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        var cell5 = row.insertCell(4);
        var cell6 = row.insertCell(5);
    
        cell1.innerHTML = movie? movie.name : movieList[contor].name;
        cell2.innerHTML = movie? movie.date : movieList[contor].date;
        cell3.innerHTML = movie? movie.genre :  movieList[contor].genre;
        cell4.innerHTML = movie? movie.rating :  movieList[contor].rating;
        cell5.innerHTML = movie? movie.dvd :  movieList[contor].dvd;
        var button = document.createElement('button');
        button.innerHTML ="Delete";
        cell6.appendChild(button);
        button.setAttribute('onclick','(DeleteRow('+row.rowIndex +', '+ contor + '))') ;
        

        

}


function DeleteRow(tableRowIndex , movieListIndex){

    document.getElementById("myTable").deleteRow(tableRowIndex);
    movieList.push(movieList.splice(movieListIndex, 1)[0]);
    movieList.pop();
    localStorage.setItem('myDataKey', JSON.stringify( movieList));




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

function SortDirection (){

    var spanAsc = document.getElementsByClassName("asc") ;
    var spanDesc = document.getElementsByClassName("desc") ;
   if( getComputedStyle(spanAsc[0]).display == 'none'){
      
    for(var i = 0; i < spanAsc.length; i++){
        
        spanAsc[i].style.display = "inline"; 
        spanDesc[i].style.display = "none";
       }
    return 'asc';
    } else
    {
   
    for(var i = 0; i < spanDesc.length; i++){
        
        spanDesc[i].style.display = "inline"; 
        spanAsc[i].style.display = "none"; 
        
       }
   return 'desc';
}
}


function SortTable(t){
    
   let ColumnValue = new Map();
   ColumnValue [0] = 'name';
   ColumnValue [1] = 'date';
   ColumnValue [2] = 'genre'
   ColumnValue [3] = 'rating';
   ColumnValue [4] = 'dvd';
 
 movieList.sort(compareValues(ColumnValue[t.cellIndex] , SortDirection () ))
   
    var table = document.getElementById("myTable");
    clearTable();
    for(var i = 0 ; i < movieList.length; i++)
    InsertDataIntoTable(i,table);
}

function compareValues(key, order = 'asc') {
    return function innerSort(a, b) {
      if (!a.hasOwnProperty(key) || !b.hasOwnProperty(key)) {
        // property doesn't exist on either object
        return 0;
      }
  
      const varA = (typeof a[key] === 'string')
        ? a[key].toUpperCase() : a[key];
      const varB = (typeof b[key] === 'string')
        ? b[key].toUpperCase() : b[key];
  
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
function clearTable(){
  var table = document.getElementById("myTable");
    for(var i = table.rows.length - 1; i > 0; i--)
    {
        table.deleteRow(i);
    }
}