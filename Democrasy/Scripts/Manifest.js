
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

// Get top n manifests from the server
var GetTopManifests = function(num) {
    $.ajax({
        url: "/Manifest/GetTopRanked/",
        data: { numOfRecords: num },
        cache: false,
        type: "GET",
        success: function (data) {
            if (jQuery.isEmptyObject(data)) {
                return false;
            }
            else {
                var result = [];

                for (var i = 0; i < data.length; i++) {
                    var currManifest = data[i];
                    var manifestObj = String.format('<div id="{0}" class="panel panel-default grid-item"> <div class="row"> <div class="col-xs-6"> <h4 class="panel-heading">{1}</h4> </div> <div class="col-xs-6"> <h4 class="panel-heading">{2}</h4> </div> </div> <p class="panel-body">{3}</p> <div class="row"> <div class="col-md-4"> <button type="button" class="btn btn-default" onClick="DownvoteManifest("{0}");"> <span class="glyphicon glyphicon-thumbs-down" aria-hidden="true"></span> </button> </div> <div class="col-md-4"> <p class="rank">{4}</p> </div> <div class="col-md-4"> <button type="button" class="btn btn-default" onClick="UpvoteManifest("{0}");"> <span class="glyphicon glyphicon-thumbs-up" aria-hidden="true"></span> </button> </div> </div> </div>',
                        currManifest.Id, currManifest.Author, currManifest.Timestamp, currManifest.Text, currManifest.Rank);
                    result.push($(manifestObj));
                }

                return result;
            }
        },
        error: function (reponse) {
            return false;
        }
    });
}