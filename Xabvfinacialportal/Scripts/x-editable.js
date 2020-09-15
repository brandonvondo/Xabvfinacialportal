(function($) {
  'use strict';
  $(function() {
    if ($('#editable-form').length) {
      $.fn.editable.defaults.mode = 'inline';
      $.fn.editableform.buttons =
        '<button type="submit" class="btn btn-primary btn-sm editable-submit">' +
        '<i class="fa fa-fw fa-check"></i>' +
        '</button>' +
        '<button type="button" class="btn btn-default btn-sm editable-cancel">' +
        '<i class="fa fa-fw fa-times"></i>' +
        '</button>';
      $('.firstname').editable({
        validate: function(value) {
          if ($.trim(value) === '') return 'This field is required';
        }
      });
    }
  });
})(jQuery);