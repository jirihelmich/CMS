jQuery('document').ready(function () {

    var defaultSearchText = jQuery("#search-text").val();

    jQuery("#search-text").click(function () {
        if (jQuery(this).val() == defaultSearchText) {
            jQuery(this).val('');
        }
    });

    jQuery("#search-text").blur(function () {
        if (jQuery(this).val() == "") {
            jQuery(this).val(defaultSearchText);
        }
    });

    jQuery('.carousel-item').click(function () {
        resetAll(this);
    });

    changeImage();

    jQuery('.carousel-content .right').tooltip();

    function resetAll(element) {
        jQuery('.selected .carousel-content').hide('fast', function () {
            jQuery('.carousel-content', element).show('slow');

            jQuery('.carousel-item').each(function () {
                jQuery(this).removeClass('selected');
            });

            jQuery(element).addClass('selected');
        });
    }

    function changeImage() {
        jQuery('.thumbnails a').click(function () {
            var src = jQuery('img', this).attr('src');

            $('.carousel-content .right > img').attr('src', src);
            jQuery('.thumbnails .selected').removeClass('selected');
            jQuery(this).addClass('selected');
            return false;
        });
        return false;
    }
});