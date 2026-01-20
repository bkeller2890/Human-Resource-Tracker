// Simple tests for date-range adapter
describe('DateRange adapter', function() {
  it('should mark form invalid when end < start', function() {
    // setup DOM fixture
    var $f = $('<form id="t1"><input id="s" name="StartDate" type="date" value="2025-12-10" /><input id="e" name="EndDate" type="date" data-val-daterange="End must be after start" data-val-daterange-start="#s" data-val-daterange-end="#e" value="2025-12-09" /></form>');
    $('body').append($f);
    $.validator.unobtrusive.parse($f);
    var valid = $f.valid();
    $f.remove();
    chai.expect(valid).to.be.false;
  });
});
