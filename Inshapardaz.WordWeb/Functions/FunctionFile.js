// The initialize function must be run each time a new page is loaded.
(function () {
    Office.initialize = function (reason) {
        // If you need to initialize something you can do so here.
    };

    function writeShadda() {

        console.log("function called");
        writeText("test");
        event.completed();
    }

    function writeText(text) {

        Office.context.document.setSelectedDataAsync(text,
            function (asyncResult) {
                var error = asyncResult.error;
                if (asyncResult.status === "failed") {
                    // Show error message. 
                }
                else {
                    // Show success message.
                }
            });
    }
})();