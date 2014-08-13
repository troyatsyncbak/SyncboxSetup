var div = $('div.autoscrolling');
var scroller = setInterval(function(){
    var pos = div.scrollTop();
    div.scrollTop(++pos);
    if($(this).scrollTop() + $(this).innerHeight() >= this.scrollHeight){
        clearInterval(scroller);
    }
}, 100)​

function smoothScroll(){
    var isScrolling;
    var scrolldelay;
    function pageScroll() {
        window.scrollBy(0, 1);
        scrolldelay = setTimeout('pageScroll()', 25);
        isScrolling = true;
    }

    if (!isScrolling) {
        pageScroll();
    } else {
        isScrolling = false; clearTimeout(scrolldelay);
    }
}