$(document).ready(function () {
    console.log("Document is ready.");

    // Event delegation for the modal close buttons
    $(document).on('click', '#modalCloseBtn, #modalCloseFooterBtn', function () {
        console.log("Close button clicked.");
        stopVideo1();
    });

    function stopVideo1() {
        var $iframe = $('#videoIframe4'); // Corrected selector for the second video
        var src = $iframe.attr('src');
        $iframe.attr('src', src);
    }

    // For safety, also handle the modal hide event
    $('#videoModal4').on('hide.bs.modal', function () {
        console.log("Modal is being hidden.");
        stopVideo1(); // Call stopVideo1 for the second video
    });

    $(document).on('click', '#modalCloseBtn1, #modalCloseFooterBtn1', function () {
        console.log("Close button clicked.");
        stopVideo2();
    });

    function stopVideo2() {
        var $iframe = $('#videoIframe1');
        var src = $iframe.attr('src');
        $iframe.attr('src', src);
    }

    // For safety, also handle the modal hide event
    $('#videoModal').on('hide.bs.modal', function () {
        console.log("Modal is being hidden.");
        stopVideo2(); // Call stopVideo2 for the first video
    });

     $(document).on('click', '#modalCloseBtn2, #modalCloseFooterBtn2', function () {
        console.log("Close button clicked.");
        stopVideo3();
    });

    function stopVideo3() {
        var $iframe = $('#videoIframe2');
        var src = $iframe.attr('src');
        $iframe.attr('src', src);
    }

    // For safety, also handle the modal hide event
    $('#videoModal1').on('hide.bs.modal', function () {
        console.log("Modal is being hidden.");
        stopVideo2(); // Call stopVideo2 for the first video
    });

    $(document).on('click', '#modalCloseBtn3, #modalCloseFooterBtn3', function () {
        console.log("Close button clicked.");
        stopVideo4();
    });

    function stopVideo4() {
        var $iframe = $('#videoIframe3');
        var src = $iframe.attr('src');
        $iframe.attr('src', src);
    }

    // For safety, also handle the modal hide event
    $('#videoModal2').on('hide.bs.modal', function () {
        console.log("Modal is being hidden.");
        stopVideo2(); // Call stopVideo2 for the first video
    });
});
