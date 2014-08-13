function StepsViewModel() {
    var self = this;

    self.selectedInstallStepId = ko.observable();
    self.selectedStation = ko.observable();
    self.selectedAccountOwner = ko.observable();
    self.selectedIsComplete = ko.observable();
    self.loading = ko.observable();

    self.steps = ko.observableArray([]);
    self.stations = ko.observableArray([]);

    self.statusItems = ["Blocked", "In Progress", "Complete"];
    self.owners = ["IT", "MAC", "Reps", "QA"];

    self.loadStations = function (ctx) {

        $.getJSON("/home/GetStations",
            function (data) {
                self.stations(data);
                if (ctx > 0) {
                    self.selectedStation(ctx);
                }
            }
        );
    }

    self.loadStepInfo = function (ctx) {
        self.loading = true;
        $.ajax({
            url: "/home/GetStepInfo",
            data: { stationId: ctx },
            type: "GET",
            dataType: "json",
            success: function (data) {
                self.selectedInstallStepId(data.InstallStepId);
                self.selectedAccountOwner(data.AccountOwner);
                self.selectedIsComplete(data.IsComplete);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("An error occurred while loading step info.");
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            },
            complete: function (jqXHR, textStatus) {
                self.loading = false;
            }
        });
    }

    self.loadInstallSteps = function (ctx) {
        self.loading = true;
        $.ajax({
            url: "/home/GetInstallSteps",
            data: { stationId: ctx },
            type: "GET",
            dataType: "json",
            success: function (data) {
                self.steps([]);
                ko.utils.arrayForEach(data, function (item) {
                    self.steps.push(new Step(item));
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("An error occurred while saving a step.");
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            },
            complete: function (jqXHR, textStatus) {
                self.loading = false;
            }
        });
    }

    self.selectedStation.subscribe(function (stationId) {
        //console.log('subscribe: ' + stationId);
        self.loadInstallSteps(stationId);
        self.loadStepInfo(stationId);
    });

    self.selectedAccountOwner.subscribe(function (newAccountOwner) {
        self.save();
    }.bind(self));

    self.selectedIsComplete.subscribe(function (isComplete) {
        self.save();
    }.bind(self));

    self.editStation = function (item) {
        console.log(item);
        window.location = "/home/edit/" + item.StationId;
    };

    self.save = function () {
        if (self.loading == true) {
            return;
        }
        
        //console.log(self.selectedInstallStepId());
        //console.log(self.selectedStation());
        //console.log(self.selectedAccountOwner());
        if (!self.selectedInstallStepId() || !self.selectedInstallStepId() > 0 || !self.selectedStation() || !self.selectedStation() > 0 || !self.selectedAccountOwner()) {
            console.log('Validation error');
            return;
        }

        var dataToSave = {
            installStepId: self.selectedInstallStepId(),
            stationId: self.selectedStation(),
            accountOwner: self.selectedAccountOwner(),
            isComplete: (self.selectedIsComplete() ? self.selectedIsComplete() : 0 )
        };
        //console.log(JSON.stringify(dataToSave));

        if (self.selectedInstallStepId() > 0) {
            $.ajax({
                url: "/home/SaveInstallStepInfo",
                data: JSON.stringify(dataToSave),
                type: "POST",
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    //console.log(data);
                    self.loadInstallSteps(self.selectedStation());
                }.bind(self),
                error: function (xhr, statusText, errorThrown) {
                    alert('statusText: ' + statusText);
                    alert('An error occured saving an install step: ' + errorThrown);
                },
            });
        }
    };

    self.scroll = function () {

    };

    self.addInitialSteps = function (item) {

        $.ajax({
            url: "/Home/AddInitialSteps",
            data: JSON.stringify({ stationId: self.selectedStation() }),
            type: "POST",
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                console.log(data);
                self.loadInstallSteps(self.selectedStation());
            }.bind(this),
            error: function (xhr, statusText, errorThrown) {
                if (errorThrown != 'abort') {
                    alert('An error occured during the add: ' + errorThrown);
                }
            },
            complete: function () { }.bind(this)
        });
    }
}

//#region Classes

function Step(data) {
    var self = this;

    self.InstallStepId = data.InstallStepId;
    self.StepId = data.StepId;
    self.StepName = data.StepName;
    self.StationId = data.StationId;
    self.CallSign = data.CallSign;
    self.CategoryName = data.CategoryName;
    self.StartDate = ko.observable(data.StartDate);
    self.EndDate = ko.observable(data.EndDate);
    self.Status = ko.observable(data.Status);
    self.Owner = ko.observable(data.Owner);
    self.AccountOwner = ko.observable(data.AccountOwner);
    self.CurrentStep = ko.observable(data.CurrentStep);

    var selected;
    if (data.CurrentStep == true) {
        selected = self.InstallStepId;
    }
    
    self.selectedStep = ko.observable(selected);

    self.StartDate.subscribe(function (newStartDate) {
        self.save();
    }.bind(self));

    self.EndDate.subscribe(function (newEndDate) {
        self.save();
    }.bind(self));

    self.Status.subscribe(function (newStatus) {
        if (newStatus != undefined) {
            self.save();
        }
    }.bind(self));

    self.Owner.subscribe(function (newOwner) {
        if (newOwner != undefined) {
            self.save();
        }
    }.bind(self));

    self.selectedStep.subscribe(function (newCurrentStep) {
        self.save();
    }.bind(self));

    self.save = function () {
        var dataToSave = {
            installStepId: self.InstallStepId,
            accountOwner: self.AccountOwner,
            stepId: self.StepId,
            startDate: self.StartDate() && self.StartDate() != '0001-01-01T00:00:00' ? new Date(self.StartDate()) : null,
            endDate: self.EndDate() && self.EndDate() != '0001-01-01T00:00:00' ? new Date(self.EndDate()) : null,
            status: self.Status() ? self.Status() : null,
            owner: self.Owner() ? self.Owner() : null,
            stationId: self.StationId,
            currentStep: self.selectedStep() ? 1 : 0
        }
        console.log(JSON.stringify(dataToSave));

        $.ajax({
            url: "/home/SaveInstallStep",
            data: JSON.stringify(dataToSave),
            type: "POST",
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                console.log(data);
            }.bind(self),
            error: function (xhr, statusText, errorThrown) {
                alert('statusText: ' + statusText);
                alert('An error occured saving an install step: ' + errorThrown);
            },
        });
    }
}

//#endregion

//#region Custom Bindings

ko.bindingHandlers.dateString = {
    init: function (element, valueAccessor, allBindings) {
        // Attach an event handler to our dom element to handle user input
        element.onchange = function () {
            // Get our observable
            var value = valueAccessor();

            // Set our observable to the parsed date from the input
            value(new Date(element.value));
            //value(moment(element.value).toDate());
        };
    },
    update: function (element, valueAccessor, allBindings) {
        var value = valueAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        if (valueUnwrapped && valueUnwrapped != '0001-01-01T00:00:00') {
            element.value = moment(valueUnwrapped).format('L');
        }
    }
};

ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || 'MM/DD/YYYY';

        var output = "";
        if (valueUnwrapped !== null && valueUnwrapped !== undefined && valueUnwrapped.length > 0 && valueUnwrapped !== '0001-01-01T00:00:00') {
            output = moment(valueUnwrapped).format(pattern);
        }

        if ($(element).is("input") === true) {
            $(element).val(output);
        } else {
            $(element).text(output);
        }
    }
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        if (valueUnwrapped && valueUnwrapped != '0001-01-01T00:00:00') {
            $(element).datepicker("setDate", moment(valueUnwrapped).format('L'));
        }
    }
};

//#endregion

var model = new StepsViewModel()
ko.applyBindings(model);

