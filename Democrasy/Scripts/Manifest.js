var manifestSkip = 0;

// Add string.format()
(function () {
    if (!String.format) {
        String.format = function (format) {
            var args = Array.prototype.slice.call(arguments, 1);
            return format.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] != 'undefined'
                  ? args[number]
                  : match
                ;
            });
        };
    }
})();

// Get newest n manifests from server and append to the DOM
var GetNewManifests = function (num) {
    GetManifests(num, "/Manifest/GetNewest/");
}

// Get top n manifests from server and append to the DOM
var GetTopManifests = function (num) {
    GetManifests(num, "/Manifest/GetTopRanked/");
}

// Get manifests from server and append to the DOM
var GetManifests = function (num, url) {
    $.ajax({
        url: url,
        data: { numOfRecords: num, skip: manifestSkip },
        cache: false,
        type: "GET",
        success: function (data) {
            if (jQuery.isEmptyObject(data)) {
                DisableGetMore();
            }
            else {
                if (data.length < num) {
                    DisableGetMore();
                }

                for (var i = 0; i < data.length; i++) {
                    var currManifest = data[i];

                    var markup = String.format('<div id="{0}" class="panel panel-default grid-item"> <div class="row"> <div class="col-xs-6"> <h4 class="panel-heading">{1}</h4> </div> <div class="col-xs-6"> <h4 class="panel-heading">{2}</h4> </div> </div> <p class="panel-body">{3}</p> <div class="row text-center rank-manage"> <div class="col-xs-4"> <button type="button" class="btn btn-default glyph-button" onClick="DownvoteManifest(\'{0}\');"> <span class="glyphicon glyphicon-thumbs-down" aria-hidden="true"></span> </button> </div> <div class="col-xs-4"> <p class="rank">{4}</p> </div> <div class="col-xs-4"> <button type="button" class="btn btn-default glyph-button" onClick="UpvoteManifest(\'{0}\');"> <span class="glyphicon glyphicon-thumbs-up" aria-hidden="true"></span> </button> </div> </div> </div>',
                        currManifest.Id, currManifest.Author, currManifest.Timestamp, currManifest.Text, currManifest.Rank);

                    var item = $(markup);

                    $('.grid').masonry()
                        .append(item)
                        .masonry('appended', item);
                }

                manifestSkip += data.length;
            }
        },
        error: function (response) {
            alert("An error accurd while trying to get manifests. Please try again later.");
            console.log("Error on GetManifests.", response);
        }
    });
}

// Upvote requested manifest
var UpvoteManifest = function (manifestId) {
    ChangeManifestRank(manifestId, "/Manifest/Upvote/", 1);
}

// Downvote requested manifest
var DownvoteManifest = function (manifestId) {
    ChangeManifestRank(manifestId, "/Manifest/Downvote/", -1);
}

var ChangeManifestRank = function (manifestId, url, change) {
    $.ajax({
        url: url,
        data: { id: manifestId },
        cache: false,
        type: "POST",
        success: function (result) {
            if (result == "True") {
                var rank = $('#' + manifestId + ' .rank').text();
                rank = parseInt(rank) + change;
                $('#' + manifestId + ' .rank').text(rank);
                $('#' + manifestId + ' :button').prop('disabled', true);
            }
            else {
                alert("Unable to change manifest's rank");
            }
        }
    });
}

var DisableGetMore = function () {
    $('#getmore span').removeClass('glyphicon-menu-down').addClass('glyphicon-minus');
    $('#getmore').prop('disabled', true)
}