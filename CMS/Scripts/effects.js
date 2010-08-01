/// <reference path="jquery-1.4.1-vsdoc.js" />

$(document).ready(function () {

    $(".delete-page").click(function () {

        if (confirm("Opravdu chcete stránku smazat?")) {

            var el = $(this);

            $.ajax({
                url: "/backend/deletePageAjax",
                type: "POST",
                dataType: 'json',
                data: { id: parseInt(el.attr('rel')) },
                //contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    data = eval("(" + data + ")");

                    if (data) {
                        el.parents("tr").fadeOut().remove();
                    } else {
                        alert("Došlo k chybě.");
                    }

                }

            });
        }

        return false;
    });

    var documentUploadInProgress = false;

    switch (true) {
        case $("#add-static-page").length > 0:
            bindAddStaticPage();
            break;

        case $("#edit-static-page").length > 0:
            bindEditStaticPage();
            break;

        case $("#add-category").length > 0:
            bindAddCategory();
            break;

        case $("#edit-category").length > 0:
            bindEditCategory();
            break;

        case $("#categories-list").length > 0:
            bindListCategories();
            break;
    }

    function bindListCategories() {
        $(".category").draggable({ revert: 'invalid' });
        $(".category").droppable({ greedy: true, drop: function () {

        }
        });

        $(".category .delete").click(function () {

            var cat = $(this).parents(".category").eq(0);

            if (confirm("Opravdu chcete kategorii smazat?")) {
                $.ajax({
                    url: "/backend/deleteCategoryAjax",
                    type: "POST",
                    dataType: 'json',
                    data: Json.toString({ id: cat.attr('id') }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {

                        var data = eval('(' + data + ')');

                        if (data.result) {
                            cat.fadeOut().remove();
                        } else {
                            alert(data.errMsg);
                        }

                    }

                });
            }

        });
    }

    function collectAddPageParams() {
        var out = [];

        $("#add-page-container>div").each(function () {
            var div = $(this);
            var lang = div.attr('id');

            var docs = [];
            var imgs = [];

            $(".file", div).each(function () {
                var item = $(this);
                var i = {
                    title: $(".name", item).text(),
                    id: item.attr('id')
                };

                if (item.parents(".docs").length > 0) {
                    docs.push(i);
                } else {
                    imgs.push(i);
                }
            });


            out.push({
                lang: lang,
                data: {
                    title: $("#title-" + lang).val(),
                    text: CKEDITOR.instances["text-" + lang].getData()
                },
                files: {
                    docs: docs,
                    images: imgs
                }
            });
        });

        return JSON.stringify({ request: out });
    }

    function collectEditPageParams() {
        var out = [];

        $("#edit-page-container>div").each(function () {
            var div = $(this);
            var lang = div.attr('id');

            var docs = [];
            var imgs = [];

            $(".file", div).each(function () {
                var item = $(this);
                var i = {
                    title: $(".name", item).text(),
                    id: item.attr('id')
                };

                if (item.parents(".docs").length > 0) {
                    docs.push(i);
                } else {
                    imgs.push(i);
                }
            });


            out.push({
                id: parseInt($("#edit-static-page").attr("rel")),
                lang: lang,
                data: {
                    title: $("#title-" + lang).val(),
                    text: CKEDITOR.instances["text-" + lang].getData()
                },
                files: {
                    docs: docs,
                    images: imgs
                }
            });
        });

        return JSON.stringify({ request: out });
    }

    function bindEditCategory() {

        window.onbeforeunload = function () {
            return "Vámi provedené změny v takovém případě nebudou uloženy!";
        }

        $("#edit-category-container #tabs").tabs();


        $("#cancel").click(function () {
            window.onbeforeunload = null;

            document.location.href = "/backend/listCategories";
        });

        $("#save").click(function () {

            $.ajax({
                url: "/backend/editCategoryAjax",
                type: "POST",
                dataType: 'json',
                data: collectEditCategoryParams(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    var data = eval('(' + data + ')');

                    if (data.result) {
                        $("#cancel").click();
                    } else {
                        alert(data.errMsg);
                    }

                }

            });
        });
    }

    function collectEditCategoryParams() {

        data = JSON.parse(collectAddCategoryParams());

        data.id = $("#edit-category").attr("rel");

        return JSON.toString(data);

    }

    function bindAddCategory() {

        window.onbeforeunload = function () {
            return "Vámi provedené změny v takovém případě nebudou uloženy!";
        }

        $("#add-category-container #tabs").tabs();


        $("#cancel").click(function () {
            window.onbeforeunload = null;

            document.location.href = "/backend/listCategories";
        });

        $("#save").click(function () {

            $.ajax({
                url: "/backend/addCategoryAjax",
                type: "POST",
                dataType: 'json',
                data: collectAddCategoryParams(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    var data = eval('(' + data + ')');

                    if (data.result) {
                        $("#cancel").click();
                    } else {
                        alert(data.errMsg);
                    }

                }

            });
        });
    }

    function collectAddCategoryParams() {
        var out = [];

        $("#add-category-container #tabs>div").each(function () {

            var lang = $(this).attr("id");

            var obj = {
                lang: lang,
                data: {
                    title: $("#title-" + lang).val(),
                    content: CKEDITOR.instances["description-" + lang].getData()
                }
            };

            out.push(obj);
        });

        var parentId = ($("#tree input").length > 0 ? $("#tree input[checked=checked]").attr('id') : null);

        return JSON.stringify({ request: out, parent: parentId });
    }

    function bindEditStaticPage() {

        window.onbeforeunload = function () {
            return "Vámi provedené změny v takovém případě nebudou uloženy!";
        }

        $("#save").click(function () {

            $.ajax({
                url: "/backend/editPageAjax",
                type: "POST",
                dataType: 'json',
                data: collectEditPageParams(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    var data = eval('(' + data + ')');

                    if (data.result) {
                        $("#cancel").click();
                    } else {
                        alert(data.errMsg);
                    }

                }

            });
        });

        $("#cancel").click(function () {
            window.onbeforeunload = null;

            document.location.href = "/backend/listPages";
        });

        $("#edit-page-container").tabs();

        window.setInterval(function () {
            $("[id^=document]").add("[id^=image]").each(function () {
                var el = $(this);

                if (!documentUploadInProgress && el.val().length > 0) {
                    el.parents("form").submit();
                    documentUploadInProgress = true;
                }

            });
        }, 500);

        $("#title-change").dialog({ modal: true, autoOpen: false });

        $("[id^=files] .edit").live('click', function () {

            var el = $(this);

            $("#title-change").dialog("option", "buttons", {
                "Hotovo": function () {
                    if ($("#title-change input").val().length > 0) {
                        $(".name", el.parents(".file")).text($("#title-change input").val());
                        $("#title-change input").val("");
                    }
                    $(this).dialog('close');
                },
                "Storno": function () {
                    $(this).dialog('close');
                }
            });
            $("#title-change").dialog('open');
        });

        $("[id^=files] .delete").live('click', function () {
            $(this).parents(".file").fadeOut().remove();
        });

        $("[id^=ajaxUploadForm]").each(function () {
            var form = $(this);

            form.ajaxForm({
                iframe: true,
                dataType: "json",
                beforeSubmit: function () {
                    form.fadeOut("fast", function () {
                        form.after('<span class="uploadInfo">Nahrávám...</span>');
                    });
                },
                success: function (result) {
                    form.resetForm();
                    documentUploadInProgress = false;
                    $(".uploadInfo").remove();
                    form.fadeIn();
                    form.next(".uploaded").eq(0).append('<span class="file"><span class="name">' + result.name + '</span><span class="edit">E</span><span class="delete">X</span></span>');
                },
                error: function (xhr, textStatus, errorThrown) {
                    form.resetForm();
                    documentUploadInProgress = false;
                    $(".uploadInfo").remove();
                    form.fadeIn();
                }
            });
        });
    }

    function bindAddStaticPage() {

        window.onbeforeunload = function () {
            return "Vámi provedené změny v takovém případě nebudou uloženy!";
        }

        $("#save").click(function () {

            $.ajax({
                url: "/backend/addPageAjax",
                type: "POST",
                dataType: 'json',
                data: collectAddPageParams(),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    if (data.result) {
                        $("#cancel").click();
                    } else {
                        alert(data.errMsg);
                    }

                }

            });
        });

        $("#cancel").click(function () {
            window.onbeforeunload = null;

            document.location.href = "/backend/listPages";
        });

        $("#add-page-container").tabs();

        window.setInterval(function () {
            $("[id^=document]").add("[id^=image]").each(function () {
                var el = $(this);

                if (!documentUploadInProgress && el.val().length > 0) {
                    el.parents("form").submit();
                    documentUploadInProgress = true;
                }

            });
        }, 500);

        $("#title-change").dialog({ modal: true, autoOpen: false });

        $("[id^=files] .edit").live('click', function () {

            var el = $(this);

            $("#title-change").dialog("option", "buttons", {
                "Hotovo": function () {
                    if ($("#title-change input").val().length > 0) {
                        $(".name", el.parents(".file")).text($("#title-change input").val());
                        $("#title-change input").val("");
                    }
                    $(this).dialog('close');
                },
                "Storno": function () {
                    $(this).dialog('close');
                }
            });
            $("#title-change").dialog('open');
        });

        $("[id^=files] .delete").live('click', function () {
            $(this).parents(".file").fadeOut().remove();
        });

        $("[id^=ajaxUploadForm]").each(function () {
            var form = $(this);

            form.ajaxForm({
                iframe: true,
                dataType: "json",
                beforeSubmit: function () {
                    form.fadeOut("fast", function () {
                        form.after('<span class="uploadInfo">Nahrávám...</span>');
                    });
                },
                success: function (result) {
                    form.resetForm();
                    documentUploadInProgress = false;
                    $(".uploadInfo").remove();
                    form.fadeIn();
                    form.next(".uploaded").eq(0).append('<span class="file"><span class="name">' + result.name + '</span><span class="edit">E</span><span class="delete">X</span></span>');
                },
                error: function (xhr, textStatus, errorThrown) {
                    form.resetForm();
                    documentUploadInProgress = false;
                    $(".uploadInfo").remove();
                    form.fadeIn();
                }
            });
        });
    }

});

/*!
* jQuery Form Plugin
* version: 2.43 (12-MAR-2010)
* @requires jQuery v1.3.2 or later
*
* Examples and documentation at: http://malsup.com/jquery/form/
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/
; (function ($) {

    /*
    Usage Note:
    -----------
    Do not use both ajaxSubmit and ajaxForm on the same form.  These
    functions are intended to be exclusive.  Use ajaxSubmit if you want
    to bind your own submit handler to the form.  For example,

    $(document).ready(function() {
    $('#myForm').bind('submit', function() {
    $(this).ajaxSubmit({
    target: '#output'
    });
    return false; // <-- important!
    });
    });

    Use ajaxForm when you want the plugin to manage all the event binding
    for you.  For example,

    $(document).ready(function() {
    $('#myForm').ajaxForm({
    target: '#output'
    });
    });

    When using ajaxForm, the ajaxSubmit function will be invoked for you
    at the appropriate time.
    */

    /**
    * ajaxSubmit() provides a mechanism for immediately submitting
    * an HTML form using AJAX.
    */
    $.fn.ajaxSubmit = function (options) {
        // fast fail if nothing selected (http://dev.jquery.com/ticket/2752)
        if (!this.length) {
            log('ajaxSubmit: skipping submit process - no element selected');
            return this;
        }

        if (typeof options == 'function')
            options = { success: options };

        var url = $.trim(this.attr('action'));
        if (url) {
            // clean url (don't include hash vaue)
            url = (url.match(/^([^#]+)/) || [])[1];
        }
        url = url || window.location.href || '';

        options = $.extend({
            url: url,
            type: this.attr('method') || 'GET',
            iframeSrc: /^https/i.test(window.location.href || '') ? 'javascript:false' : 'about:blank'
        }, options || {});

        // hook for manipulating the form data before it is extracted;
        // convenient for use with rich editors like tinyMCE or FCKEditor
        var veto = {};
        this.trigger('form-pre-serialize', [this, options, veto]);
        if (veto.veto) {
            log('ajaxSubmit: submit vetoed via form-pre-serialize trigger');
            return this;
        }

        // provide opportunity to alter form data before it is serialized
        if (options.beforeSerialize && options.beforeSerialize(this, options) === false) {
            log('ajaxSubmit: submit aborted via beforeSerialize callback');
            return this;
        }

        var a = this.formToArray(options.semantic);
        if (options.data) {
            options.extraData = options.data;
            for (var n in options.data) {
                if (options.data[n] instanceof Array) {
                    for (var k in options.data[n])
                        a.push({ name: n, value: options.data[n][k] });
                }
                else
                    a.push({ name: n, value: options.data[n] });
            }
        }

        // give pre-submit callback an opportunity to abort the submit
        if (options.beforeSubmit && options.beforeSubmit(a, this, options) === false) {
            log('ajaxSubmit: submit aborted via beforeSubmit callback');
            return this;
        }

        // fire vetoable 'validate' event
        this.trigger('form-submit-validate', [a, this, options, veto]);
        if (veto.veto) {
            log('ajaxSubmit: submit vetoed via form-submit-validate trigger');
            return this;
        }

        var q = $.param(a);

        if (options.type.toUpperCase() == 'GET') {
            options.url += (options.url.indexOf('?') >= 0 ? '&' : '?') + q;
            options.data = null;  // data is null for 'get'
        }
        else
            options.data = q; // data is the query string for 'post'

        var $form = this, callbacks = [];
        if (options.resetForm) callbacks.push(function () { $form.resetForm(); });
        if (options.clearForm) callbacks.push(function () { $form.clearForm(); });

        // perform a load on the target only if dataType is not provided
        if (!options.dataType && options.target) {
            var oldSuccess = options.success || function () { };
            callbacks.push(function (data) {
                var fn = options.replaceTarget ? 'replaceWith' : 'html';
                $(options.target)[fn](data).each(oldSuccess, arguments);
            });
        }
        else if (options.success)
            callbacks.push(options.success);

        options.success = function (data, status, xhr) { // jQuery 1.4+ passes xhr as 3rd arg
            for (var i = 0, max = callbacks.length; i < max; i++)
                callbacks[i].apply(options, [data, status, xhr || $form, $form]);
        };

        // are there files to upload?
        var files = $('input:file', this).fieldValue();
        var found = false;
        for (var j = 0; j < files.length; j++)
            if (files[j])
                found = true;

        var multipart = false;
        //	var mp = 'multipart/form-data';
        //	multipart = ($form.attr('enctype') == mp || $form.attr('encoding') == mp);

        // options.iframe allows user to force iframe mode
        // 06-NOV-09: now defaulting to iframe mode if file input is detected
        if ((files.length && options.iframe !== false) || options.iframe || found || multipart) {
            // hack to fix Safari hang (thanks to Tim Molendijk for this)
            // see:  http://groups.google.com/group/jquery-dev/browse_thread/thread/36395b7ab510dd5d
            if (options.closeKeepAlive)
                $.get(options.closeKeepAlive, fileUpload);
            else
                fileUpload();
        }
        else
            $.ajax(options);

        // fire 'notify' event
        this.trigger('form-submit-notify', [this, options]);
        return this;


        // private function for handling file uploads (hat tip to YAHOO!)
        function fileUpload() {
            var form = $form[0];

            if ($(':input[name=submit]', form).length) {
                alert('Error: Form elements must not be named "submit".');
                return;
            }

            var opts = $.extend({}, $.ajaxSettings, options);
            var s = $.extend(true, {}, $.extend(true, {}, $.ajaxSettings), opts);

            var id = 'jqFormIO' + (new Date().getTime());
            var $io = $('<iframe id="' + id + '" name="' + id + '" src="' + opts.iframeSrc + '" onload="(jQuery(this).data(\'form-plugin-onload\'))()" />');
            var io = $io[0];

            $io.css({ position: 'absolute', top: '-1000px', left: '-1000px' });

            var xhr = { // mock object
                aborted: 0,
                responseText: null,
                responseXML: null,
                status: 0,
                statusText: 'n/a',
                getAllResponseHeaders: function () { },
                getResponseHeader: function () { },
                setRequestHeader: function () { },
                abort: function () {
                    this.aborted = 1;
                    $io.attr('src', opts.iframeSrc); // abort op in progress
                }
            };

            var g = opts.global;
            // trigger ajax global events so that activity/block indicators work like normal
            if (g && !$.active++) $.event.trigger("ajaxStart");
            if (g) $.event.trigger("ajaxSend", [xhr, opts]);

            if (s.beforeSend && s.beforeSend(xhr, s) === false) {
                s.global && $.active--;
                return;
            }
            if (xhr.aborted)
                return;

            var cbInvoked = false;
            var timedOut = 0;

            // add submitting element to data if we know it
            var sub = form.clk;
            if (sub) {
                var n = sub.name;
                if (n && !sub.disabled) {
                    opts.extraData = opts.extraData || {};
                    opts.extraData[n] = sub.value;
                    if (sub.type == "image") {
                        opts.extraData[n + '.x'] = form.clk_x;
                        opts.extraData[n + '.y'] = form.clk_y;
                    }
                }
            }

            // take a breath so that pending repaints get some cpu time before the upload starts
            function doSubmit() {
                // make sure form attrs are set
                var t = $form.attr('target'), a = $form.attr('action');

                // update form attrs in IE friendly way
                form.setAttribute('target', id);
                if (form.getAttribute('method') != 'POST')
                    form.setAttribute('method', 'POST');
                if (form.getAttribute('action') != opts.url)
                    form.setAttribute('action', opts.url);

                // ie borks in some cases when setting encoding
                if (!opts.skipEncodingOverride) {
                    $form.attr({
                        encoding: 'multipart/form-data',
                        enctype: 'multipart/form-data'
                    });
                }

                // support timout
                if (opts.timeout)
                    setTimeout(function () { timedOut = true; cb(); }, opts.timeout);

                // add "extra" data to form if provided in options
                var extraInputs = [];
                try {
                    if (opts.extraData)
                        for (var n in opts.extraData)
                            extraInputs.push(
							$('<input type="hidden" name="' + n + '" value="' + opts.extraData[n] + '" />')
								.appendTo(form)[0]);

                    // add iframe to doc and submit the form
                    $io.appendTo('body');
                    $io.data('form-plugin-onload', cb);
                    form.submit();
                }
                finally {
                    // reset attrs and remove "extra" input elements
                    form.setAttribute('action', a);
                    t ? form.setAttribute('target', t) : $form.removeAttr('target');
                    $(extraInputs).remove();
                }
            };

            if (opts.forceSync)
                doSubmit();
            else
                setTimeout(doSubmit, 10); // this lets dom updates render

            var domCheckCount = 100;

            function cb() {
                if (cbInvoked)
                    return;

                var ok = true;
                try {
                    if (timedOut) throw 'timeout';
                    // extract the server response from the iframe
                    var data, doc;

                    doc = io.contentWindow ? io.contentWindow.document : io.contentDocument ? io.contentDocument : io.document;

                    var isXml = opts.dataType == 'xml' || doc.XMLDocument || $.isXMLDoc(doc);
                    log('isXml=' + isXml);
                    if (!isXml && (doc.body == null || doc.body.innerHTML == '')) {
                        if (--domCheckCount) {
                            // in some browsers (Opera) the iframe DOM is not always traversable when
                            // the onload callback fires, so we loop a bit to accommodate
                            log('requeing onLoad callback, DOM not available');
                            setTimeout(cb, 250);
                            return;
                        }
                        log('Could not access iframe DOM after 100 tries.');
                        return;
                    }

                    log('response detected');
                    cbInvoked = true;
                    xhr.responseText = doc.body ? doc.body.innerHTML : null;
                    xhr.responseXML = doc.XMLDocument ? doc.XMLDocument : doc;
                    xhr.getResponseHeader = function (header) {
                        var headers = { 'content-type': opts.dataType };
                        return headers[header];
                    };

                    if (opts.dataType == 'json' || opts.dataType == 'script') {
                        // see if user embedded response in textarea
                        var ta = doc.getElementsByTagName('textarea')[0];
                        if (ta)
                            xhr.responseText = ta.value;
                        else {
                            // account for browsers injecting pre around json response
                            var pre = doc.getElementsByTagName('pre')[0];
                            if (pre)
                                xhr.responseText = pre.innerHTML;
                        }
                    }
                    else if (opts.dataType == 'xml' && !xhr.responseXML && xhr.responseText != null) {
                        xhr.responseXML = toXml(xhr.responseText);
                    }
                    data = $.httpData(xhr, opts.dataType);
                }
                catch (e) {
                    log('error caught:', e);
                    ok = false;
                    xhr.error = e;
                    $.handleError(opts, xhr, 'error', e);
                }

                // ordering of these callbacks/triggers is odd, but that's how $.ajax does it
                if (ok) {
                    opts.success(data, 'success');
                    if (g) $.event.trigger("ajaxSuccess", [xhr, opts]);
                }
                if (g) $.event.trigger("ajaxComplete", [xhr, opts]);
                if (g && ! --$.active) $.event.trigger("ajaxStop");
                if (opts.complete) opts.complete(xhr, ok ? 'success' : 'error');

                // clean up
                setTimeout(function () {
                    $io.removeData('form-plugin-onload');
                    $io.remove();
                    xhr.responseXML = null;
                }, 100);
            };

            function toXml(s, doc) {
                if (window.ActiveXObject) {
                    doc = new ActiveXObject('Microsoft.XMLDOM');
                    doc.async = 'false';
                    doc.loadXML(s);
                }
                else
                    doc = (new DOMParser()).parseFromString(s, 'text/xml');
                return (doc && doc.documentElement && doc.documentElement.tagName != 'parsererror') ? doc : null;
            };
        };
    };

    /**
    * ajaxForm() provides a mechanism for fully automating form submission.
    *
    * The advantages of using this method instead of ajaxSubmit() are:
    *
    * 1: This method will include coordinates for <input type="image" /> elements (if the element
    *	is used to submit the form).
    * 2. This method will include the submit element's name/value data (for the element that was
    *	used to submit the form).
    * 3. This method binds the submit() method to the form for you.
    *
    * The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
    * passes the options argument along after properly binding events for submit elements and
    * the form itself.
    */
    $.fn.ajaxForm = function (options) {
        return this.ajaxFormUnbind().bind('submit.form-plugin', function (e) {
            e.preventDefault();
            $(this).ajaxSubmit(options);
        }).bind('click.form-plugin', function (e) {
            var target = e.target;
            var $el = $(target);
            if (!($el.is(":submit,input:image"))) {
                // is this a child element of the submit el?  (ex: a span within a button)
                var t = $el.closest(':submit');
                if (t.length == 0)
                    return;
                target = t[0];
            }
            var form = this;
            form.clk = target;
            if (target.type == 'image') {
                if (e.offsetX != undefined) {
                    form.clk_x = e.offsetX;
                    form.clk_y = e.offsetY;
                } else if (typeof $.fn.offset == 'function') { // try to use dimensions plugin
                    var offset = $el.offset();
                    form.clk_x = e.pageX - offset.left;
                    form.clk_y = e.pageY - offset.top;
                } else {
                    form.clk_x = e.pageX - target.offsetLeft;
                    form.clk_y = e.pageY - target.offsetTop;
                }
            }
            // clear form vars
            setTimeout(function () { form.clk = form.clk_x = form.clk_y = null; }, 100);
        });
    };

    // ajaxFormUnbind unbinds the event handlers that were bound by ajaxForm
    $.fn.ajaxFormUnbind = function () {
        return this.unbind('submit.form-plugin click.form-plugin');
    };

    /**
    * formToArray() gathers form element data into an array of objects that can
    * be passed to any of the following ajax functions: $.get, $.post, or load.
    * Each object in the array has both a 'name' and 'value' property.  An example of
    * an array for a simple login form might be:
    *
    * [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
    *
    * It is this array that is passed to pre-submit callback functions provided to the
    * ajaxSubmit() and ajaxForm() methods.
    */
    $.fn.formToArray = function (semantic) {
        var a = [];
        if (this.length == 0) return a;

        var form = this[0];
        var els = semantic ? form.getElementsByTagName('*') : form.elements;
        if (!els) return a;
        for (var i = 0, max = els.length; i < max; i++) {
            var el = els[i];
            var n = el.name;
            if (!n) continue;

            if (semantic && form.clk && el.type == "image") {
                // handle image inputs on the fly when semantic == true
                if (!el.disabled && form.clk == el) {
                    a.push({ name: n, value: $(el).val() });
                    a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
                }
                continue;
            }

            var v = $.fieldValue(el, true);
            if (v && v.constructor == Array) {
                for (var j = 0, jmax = v.length; j < jmax; j++)
                    a.push({ name: n, value: v[j] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: n, value: v });
        }

        if (!semantic && form.clk) {
            // input type=='image' are not found in elements array! handle it here
            var $input = $(form.clk), input = $input[0], n = input.name;
            if (n && !input.disabled && input.type == 'image') {
                a.push({ name: n, value: $input.val() });
                a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
            }
        }
        return a;
    };

    /**
    * Serializes form data into a 'submittable' string. This method will return a string
    * in the format: name1=value1&amp;name2=value2
    */
    $.fn.formSerialize = function (semantic) {
        //hand off to jQuery.param for proper encoding
        return $.param(this.formToArray(semantic));
    };

    /**
    * Serializes all field elements in the jQuery object into a query string.
    * This method will return a string in the format: name1=value1&amp;name2=value2
    */
    $.fn.fieldSerialize = function (successful) {
        var a = [];
        this.each(function () {
            var n = this.name;
            if (!n) return;
            var v = $.fieldValue(this, successful);
            if (v && v.constructor == Array) {
                for (var i = 0, max = v.length; i < max; i++)
                    a.push({ name: n, value: v[i] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: this.name, value: v });
        });
        //hand off to jQuery.param for proper encoding
        return $.param(a);
    };

    /**
    * Returns the value(s) of the element in the matched set.  For example, consider the following form:
    *
    *  <form><fieldset>
    *	  <input name="A" type="text" />
    *	  <input name="A" type="text" />
    *	  <input name="B" type="checkbox" value="B1" />
    *	  <input name="B" type="checkbox" value="B2"/>
    *	  <input name="C" type="radio" value="C1" />
    *	  <input name="C" type="radio" value="C2" />
    *  </fieldset></form>
    *
    *  var v = $(':text').fieldValue();
    *  // if no values are entered into the text inputs
    *  v == ['','']
    *  // if values entered into the text inputs are 'foo' and 'bar'
    *  v == ['foo','bar']
    *
    *  var v = $(':checkbox').fieldValue();
    *  // if neither checkbox is checked
    *  v === undefined
    *  // if both checkboxes are checked
    *  v == ['B1', 'B2']
    *
    *  var v = $(':radio').fieldValue();
    *  // if neither radio is checked
    *  v === undefined
    *  // if first radio is checked
    *  v == ['C1']
    *
    * The successful argument controls whether or not the field element must be 'successful'
    * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
    * The default value of the successful argument is true.  If this value is false the value(s)
    * for each element is returned.
    *
    * Note: This method *always* returns an array.  If no valid value can be determined the
    *	   array will be empty, otherwise it will contain one or more values.
    */
    $.fn.fieldValue = function (successful) {
        for (var val = [], i = 0, max = this.length; i < max; i++) {
            var el = this[i];
            var v = $.fieldValue(el, successful);
            if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length))
                continue;
            v.constructor == Array ? $.merge(val, v) : val.push(v);
        }
        return val;
    };

    /**
    * Returns the value of the field element.
    */
    $.fieldValue = function (el, successful) {
        var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
        if (typeof successful == 'undefined') successful = true;

        if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
		(t == 'checkbox' || t == 'radio') && !el.checked ||
		(t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
		tag == 'select' && el.selectedIndex == -1))
            return null;

        if (tag == 'select') {
            var index = el.selectedIndex;
            if (index < 0) return null;
            var a = [], ops = el.options;
            var one = (t == 'select-one');
            var max = (one ? index + 1 : ops.length);
            for (var i = (one ? index : 0); i < max; i++) {
                var op = ops[i];
                if (op.selected) {
                    var v = op.value;
                    if (!v) // extra pain for IE...
                        v = (op.attributes && op.attributes['value'] && !(op.attributes['value'].specified)) ? op.text : op.value;
                    if (one) return v;
                    a.push(v);
                }
            }
            return a;
        }
        return el.value;
    };

    /**
    * Clears the form data.  Takes the following actions on the form's input fields:
    *  - input text fields will have their 'value' property set to the empty string
    *  - select elements will have their 'selectedIndex' property set to -1
    *  - checkbox and radio inputs will have their 'checked' property set to false
    *  - inputs of type submit, button, reset, and hidden will *not* be effected
    *  - button elements will *not* be effected
    */
    $.fn.clearForm = function () {
        return this.each(function () {
            $('input,select,textarea', this).clearFields();
        });
    };

    /**
    * Clears the selected form elements.
    */
    $.fn.clearFields = $.fn.clearInputs = function () {
        return this.each(function () {
            var t = this.type, tag = this.tagName.toLowerCase();
            if (t == 'text' || t == 'password' || tag == 'textarea')
                this.value = '';
            else if (t == 'checkbox' || t == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = -1;
        });
    };

    /**
    * Resets the form data.  Causes all form elements to be reset to their original value.
    */
    $.fn.resetForm = function () {
        return this.each(function () {
            // guard against an input with the name of 'reset'
            // note that IE reports the reset function as an 'object'
            if (typeof this.reset == 'function' || (typeof this.reset == 'object' && !this.reset.nodeType))
                this.reset();
        });
    };

    /**
    * Enables or disables any matching elements.
    */
    $.fn.enable = function (b) {
        if (b == undefined) b = true;
        return this.each(function () {
            this.disabled = !b;
        });
    };

    /**
    * Checks/unchecks any matching checkboxes or radio buttons and
    * selects/deselects and matching option elements.
    */
    $.fn.selected = function (select) {
        if (select == undefined) select = true;
        return this.each(function () {
            var t = this.type;
            if (t == 'checkbox' || t == 'radio')
                this.checked = select;
            else if (this.tagName.toLowerCase() == 'option') {
                var $sel = $(this).parent('select');
                if (select && $sel[0] && $sel[0].type == 'select-one') {
                    // deselect all other options
                    $sel.find('option').selected(false);
                }
                this.selected = select;
            }
        });
    };

    // helper fn for console logging
    // set $.fn.ajaxSubmit.debug to true to enable debug logging
    function log() {
        if ($.fn.ajaxSubmit.debug) {
            var msg = '[jquery.form] ' + Array.prototype.join.call(arguments, '');
            if (window.console && window.console.log)
                window.console.log(msg);
            else if (window.opera && window.opera.postError)
                window.opera.postError(msg);
        }
    };

})(jQuery);


/******************************************/

/*
    http://www.JSON.org/json2.js
    2010-03-20

    Public Domain.

    NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

    See http://www.JSON.org/js.html


    This code should be minified before deployment.
    See http://javascript.crockford.com/jsmin.html

    USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD CODE FROM SERVERS YOU DO
    NOT CONTROL.


    This file creates a global JSON object containing two methods: stringify
    and parse.

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, strict: false */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

if (!this.JSON) {
    this.JSON = {};
}

(function () {

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                   this.getUTCFullYear()   + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate())      + 'T' +
                 f(this.getUTCHours())     + ':' +
                 f(this.getUTCMinutes())   + ':' +
                 f(this.getUTCSeconds())   + 'Z' : null;
        };

        String.prototype.toJSON =
        Number.prototype.toJSON =
        Boolean.prototype.toJSON = function (key) {
            return this.valueOf();
        };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"' : '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

// If the string contains no control characters, no quote characters, and no
// backslash characters, then we can safely slap some quotes around it.
// Otherwise we must also replace the offending characters with safe escape
// sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ?
            '"' + string.replace(escapable, function (a) {
                var c = meta[a];
                return typeof c === 'string' ? c :
                    '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            }) + '"' :
            '"' + string + '"';
    }


    function str(key, holder) {

// Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

// If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

// If we were called with a replacer function, then call the replacer to
// obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

// What happens next depends on the value's type.

        switch (typeof value) {
        case 'string':
            return quote(value);

        case 'number':

// JSON numbers must be finite. Encode non-finite numbers as null.

            return isFinite(value) ? String(value) : 'null';

        case 'boolean':
        case 'null':

// If the value is a boolean or null, convert it to a string. Note:
// typeof null does not produce 'null'. The case is included here in
// the remote chance that this gets fixed someday.

            return String(value);

// If the type is 'object', we might be dealing with an object or an array or
// null.

        case 'object':

// Due to a specification blunder in ECMAScript, typeof null is 'object',
// so watch out for that case.

            if (!value) {
                return 'null';
            }

// Make an array to hold the partial results of stringifying this object value.

            gap += indent;
            partial = [];

// Is the value an array?

            if (Object.prototype.toString.apply(value) === '[object Array]') {

// The value is an array. Stringify every element. Use null as a placeholder
// for non-JSON values.

                length = value.length;
                for (i = 0; i < length; i += 1) {
                    partial[i] = str(i, value) || 'null';
                }

// Join all of the elements together, separated with commas, and wrap them in
// brackets.

                v = partial.length === 0 ? '[]' :
                    gap ? '[\n' + gap +
                            partial.join(',\n' + gap) + '\n' +
                                mind + ']' :
                          '[' + partial.join(',') + ']';
                gap = mind;
                return v;
            }

// If the replacer is an array, use it to select the members to be stringified.

            if (rep && typeof rep === 'object') {
                length = rep.length;
                for (i = 0; i < length; i += 1) {
                    k = rep[i];
                    if (typeof k === 'string') {
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            } else {

// Otherwise, iterate through all of the keys in the object.

                for (k in value) {
                    if (Object.hasOwnProperty.call(value, k)) {
                        v = str(k, value);
                        if (v) {
                            partial.push(quote(k) + (gap ? ': ' : ':') + v);
                        }
                    }
                }
            }

// Join all of the member texts together, separated with commas,
// and wrap them in braces.

            v = partial.length === 0 ? '{}' :
                gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' +
                        mind + '}' : '{' + partial.join(',') + '}';
            gap = mind;
            return v;
        }
    }

// If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

// The stringify method takes a value and an optional replacer, and an optional
// space parameter, and returns a JSON text. The replacer can be a function
// that can replace values, or an array of strings that will select the keys.
// A default replacer method can be provided. Use of the space parameter can
// produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

// If the space parameter is a number, make an indent string containing that
// many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

// If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

// If there is a replacer, it must be a function or an array.
// Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                     typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

// Make a fake root object containing our value under the key of ''.
// Return the result of stringifying the value.

            return str('', {'': value});
        };
    }


// If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

// The parse method takes a text and an optional reviver function, and returns
// a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

// The walk method is used to recursively walk the resulting structure so
// that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


// Parsing happens in four stages. In the first stage, we replace certain
// Unicode characters with escape sequences. JavaScript handles many characters
// incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

// In the second stage, we run the text against regular expressions that look
// for non-JSON patterns. We are especially concerned with '()' and 'new'
// because they can cause invocation, and '=' because it can cause mutation.
// But just to be safe, we want to reject all unexpected forms.

// We split the second stage into 4 regexp operations in order to work around
// crippling inefficiencies in IE's and Safari's regexp engines. First we
// replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
// replace all simple value tokens with ']' characters. Third, we delete all
// open brackets that follow a colon or comma or that begin the text. Finally,
// we look to see that the remaining characters are only whitespace or ']' or
// ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/.
test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').
replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

// In the third stage we use the eval function to compile the text into a
// JavaScript structure. The '{' operator is subject to a syntactic ambiguity
// in JavaScript: it can begin a block or an object literal. We wrap the text
// in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

// In the optional fourth stage, we recursively walk the new structure, passing
// each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({'': j}, '') : j;
            }

// If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
}());