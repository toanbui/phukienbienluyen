function FormDataCustom() {
    this.dict = {};
};

FormDataCustom.prototype.append = function (key, value) {
    this.dict[key] = value;
};

FormDataCustom.prototype.contains = function (key) {
    return this.dict.hasOwnProperty(key);
};

FormDataCustom.prototype.getValue = function (key) {
    return this.dict[key];
};

FormDataCustom.prototype.valueOf = function () {
    var fd = new FormData();
    for (var key in this.dict) {
        if (this.dict.hasOwnProperty(key))
            fd.append(key, this.dict[key]);
    }

    return fd;
};
FormDataCustom.prototype.del = function (keyremove) {
    var fd = new FormDataCustom();
    for (var key in this.dict) {
        if (key != keyremove) {
            if (this.dict.hasOwnProperty(key))
                fd.append(key, this.dict[key]);
        }
    }
    return fd;
};

FormDataCustom.prototype.filter = function (arrkeyadd) {
    var fd = new FormDataCustom();
    for (var key in this.dict) {
        for (var i = 0 ; i < arrkeyadd.length ; i++) {
            if (key == arrkeyadd[i]) {
                if (this.dict.hasOwnProperty(key))
                    fd.append(key, this.dict[key]);
            }
        }
    }
    return fd;
};

FormDataCustom.prototype.safe = function () {
    return this.valueOf();
};
FormDataCustom.prototype.addObject = function (object) {
    var formData = this.valueOf();
    for (var key in object) {
        formData.append(key, object[key]);
    }
    return formData;
};