var serializeForm = function (form) {
    var obj = {};
    var formData = new FormData(form);
    for (var key of formData.keys()) {
        obj[key] = formData.get(key);
    }
    return obj;
};


function httpPost(uri, body, token = '') {

    return (
        fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify(body)
        }).then((response) => {
            if (response.ok === true) {
                console.log('Se entro al OK de la promesa, valor: ', response);
                return response.json()
                }
            else
                throw new Error(`Error al consumir servicio ${uri}: ${response.statusText}`);
        }).catch((error) => {
            return error;
        })
    );
}



function httpPostFile(uri, formDataFile, token = '') {

    return (
        fetch(uri, {
            method: 'POST',
            body: formDataFile
        }).then((response) => {
            return response.json()
        }).catch((error) => {
            return error;
        })
    );
}



function httpGet(uri, params = null, token = '') {

    let extraParams = '';
    let primerParam = true;
    if (params !== null) {
        const lengthParams = params.length;
        for (let i = 0; i < lengthParams; i++) {
            if (primerParam) {
                extraParams += `?${params[i].key}=${params[i].value}`;
                primerParam = false;
            }
            else
                extraParams += `&${params[i].key}=${params[i].value}`;
        }
    }

    const completeUrl = uri + extraParams;
    return fetch(
        completeUrl, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },

    })
        .then(response => {
            console.log(response);
        return response.json();
    })
    .catch(error => {
        return error;
    });
}




function isANullOrEmptyString(myStr) {
    let result = false;
    if (myStr === null)
        result = true;
    else
        if (typeof myStr === "string" && myStr.trim().length === 0)
            result = true;

    return result;
}

function isANullOrUndefinedElement(element) {
    let result = true;
    if (typeof (element) !== 'undefined' && element !== null) {
        result = false;
    }

    return result;
}


function postForm(path, params, method) {
    method = method || 'post';

    var form = document.createElement('form');
    form.setAttribute('method', method);
    form.setAttribute('action', path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement('input');
            hiddenField.setAttribute('type', 'hidden');
            hiddenField.setAttribute('name', key);
            hiddenField.setAttribute('value', params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}



function setErrorMessageForm(message, divId) {
    let css = 'alert-danger';
    let divHTMLContent = '<div class="alert ' + css + ' alert-dismissable" style="margin-right: 60px;"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + message + '</div>';
    let divMensajeError = document.getElementById(divId);
    divMensajeError.innerHTML = '';
    divMensajeError.innerHTML = divHTMLContent;

}


function setAlertMessageForm(message, divId) {
    let css = 'alert-warning';
    let divHTMLContent = '<div class="alert ' + css + ' alert-dismissable" style="margin-right: 60px;"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + message + '</div>';
    let divMensajeError = document.getElementById(divId);
    divMensajeError.innerHTML = '';
    divMensajeError.innerHTML = divHTMLContent;

}

function setSuccessMessageForm(message, divId) {
    let css = 'alert-success';
    let divHTMLContent = '<div class="alert ' + css + ' alert-dismissable" style="margin-right: 60px;"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + message + '</div>';
    let divMensajeError = document.getElementById(divId);
    divMensajeError.innerHTML = '';
    divMensajeError.innerHTML = divHTMLContent;

}

function cleanElementHTML(divId) {
    let divMensajeError = document.getElementById(divId);
    divMensajeError.innerHTML = '';

}
function setNewValueHTML(htmlValue, divId) {
    let divMensajeError = document.getElementById(divId);
    divMensajeError.innerHTML = '';
    divMensajeError.innerHTML = htmlValue;

}
function showLoader() {
    const preloader = document.querySelector(".preloader");
    preloader.style.display = "block";
}
function hideLoader() {
    const preloader = document.querySelector(".preloader");
    preloader.style.display = "none";
}

