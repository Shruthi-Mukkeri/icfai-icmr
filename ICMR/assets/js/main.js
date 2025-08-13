/**
    * Easy on scroll event listener 
    */
const onscroll = (el, listener) => {
    el.addEventListener('scroll', listener)
}

const select = (el, all = false) => {
    el = el.trim();
    if (all) {
        return [...document.querySelectorAll(el)];
    } else {
        return document.querySelector(el);
    }
};

/**
 * Back to top button
 */
let backtotop = select('.back-to-top')
if (backtotop) {
    const toggleBacktotop = () => {
        if (window.scrollY > 100) {
            backtotop.classList.add('active')
        } else {
            backtotop.classList.remove('active')
        }
    }
    window.addEventListener('load', toggleBacktotop)
    onscroll(document, toggleBacktotop)
}

$(".testimonialSpeak").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 1,
            nav: false
        }
    }
});



$(".carouselAwardlogs").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 4,
            nav: false
        }
    }
});
$(".carouselAwardlogs1").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 1,
            nav: false
        }
    }
});

 
$(".carouselAcademiclogs-new").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 3,
            nav: false
        },
        1000: {
            items: 5,
            nav: false
        }
    }
});

$(".carouselAcademiclogs").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 2,
            nav: false
        },
        1000: {
            items: 3,
            nav: false
        }
    }
});

$(".carouselPublisherlogs").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 2,
            nav: false
        },
        1000: {
            items: 5,
            nav: false
        }
    }
});


//Leadcapture Form
function openLeadCaptureForm() {
    document.getElementById("leadCaptureForm").style.cssText = "right: 0px;";
}
function closeLeadCaptureForm() {
    document.getElementById("leadCaptureForm").style.cssText = "right: -400px;";
}

function toggleVisibility() {
    var element = document.getElementById("elementToToggle");
    element.classList.toggle("hidden");
    //document.body.addEventListener('onclick', toggleVisibility)
}




(function () {
    "use strict";

    /**
     * Easy selector helper function
     */
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    /**
     * Easy event listener function
     */
    const on = (type, el, listener, all = false) => {
        if (all) {
            select(el, all).forEach(e => e.addEventListener(type, listener))
        } else {
            select(el, all).addEventListener(type, listener)
        }
    }

    /**
     * Easy on scroll event listener 
     */
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    /**
     * Navbar links active state on scroll
     */
    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    /**
     * Scrolls to an element with header offset
     */
    const scrollto = (el) => {
        let header = select('#header')
        let offset = header.offsetHeight

        if (!header.classList.contains('header-scrolled')) {
            offset -= 10
        }

        let elementPos = select(el).offsetTop
        window.scrollTo({
            top: elementPos - offset,
            behavior: 'smooth'
        })
    }

    /**
     * Toggle .header-scrolled class to #header when page is scrolled
     */
    let selectHeader = select('#header')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
            } else {
                selectHeader.classList.remove('header-scrolled')
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }
 
    /**
     * Mobile nav toggle
     */
    on('click', '.mobile-nav-toggle', function (e) {
        select('#navbar').classList.toggle('navbar-mobile')
        this.classList.toggle('bi-list')
        this.classList.toggle('bi-x')
    })

    /**
     * Mobile nav dropdowns activate
     */
    on('click', '.navbar .dropdown > a', function (e) {
        if (select('#navbar').classList.contains('navbar-mobile')) {
            e.preventDefault()
            this.nextElementSibling.classList.toggle('dropdown-active')
        }
    }, true)

    /**
     * Scrool with ofset on links with a class name .scrollto
     */
    on('click', '.scrollto', function (e) {
        if (select(this.hash)) {
            e.preventDefault()

            let navbar = select('#navbar')
            if (navbar.classList.contains('navbar-mobile')) {
                navbar.classList.remove('navbar-mobile')
                let navbarToggle = select('.mobile-nav-toggle')
                navbarToggle.classList.toggle('bi-list')
                navbarToggle.classList.toggle('bi-x')
            }
            scrollto(this.hash)
        }
    }, true)

    /**
     * Scroll with ofset on page load with hash links in the url
     */
    window.addEventListener('load', () => {
        if (window.location.hash) {
            if (select(window.location.hash)) {
                scrollto(window.location.hash)
            }
        }
    });


    /**
     * Animation on scroll
     */
    function aos_init() {
        AOS.init({
            duration: 1000,
            easing: "ease-in-out",
            once: true,
            mirror: false
        });
    }
    window.addEventListener('load', () => {
        aos_init();
    });


    /**
 * Initiate Pure Counter 
 */
    new PureCounter();


})();



document.querySelectorAll('.readmore').forEach(function (item) {
    item.addEventListener("click", function () {
        var p = this.parentElement;
        var x = p.previousElementSibling;
        if (x.style.display === "none") {
            x.style.display = "inline";
            this.innerHTML = "Read Less"
        } else {
            x.style.display = "none";
            this.innerHTML = "Read More"
        }
    })
})







 

$(".carouselPlacemnetsList").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 1,
            nav: false
        }
    }
});

$(".carouselAcademiclogs1").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 2,
            nav: false
        },
        1000: {
            items: 3,
            nav: false
        }
    }
});


$('.carouselLeaderShip').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    autoplay: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 2
        }
    }
})




$('.owl-starPlacements').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    autoplay: true,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 3
        },
        1000: {
            items: 4
        }
    }
})


$(".carouselAumni").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
            nav: false
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 2,
            nav: false
        }
    }
});

$('.alumni-carousel').owlCarousel({
    loop: true,
    items: 1,
    margin: 0,
    stagePadding: 0,
    smartSpeed: 450,
    nav: true,
});

// Function to toggle between light and dark themes
//function toggleTheme() {
    // Get the current theme class of the body
 //   const htmlClass = document.html.classList.contains('light') ? 'light' : 'dark';
  
    // Toggle the class based on the current theme
   // if (bodyClass === 'light') {
      //  document.html.classList.remove('light');
     //   document.html.classList.add('dark');
      
       
   // } else {
    //    document.html.classList.remove('dark');
    //   document.html.classList.add('light');

   // }

    // Save user preference (optional)
   // saveThemePreference(htmlClass === 'light' ? 'dark' : 'light');
//}

// Function to save user theme preference (optional)
function saveThemePreference(theme) {
    localStorage.setItem('preferredTheme', theme);
}

// Function to load user theme preference (optional)
function loadThemePreference() {
    const preferredTheme = localStorage.getItem('preferredTheme');
    if (preferredTheme === 'dark') {
        document.body.classList.remove('light');
        document.body.classList.add('dark');
    }
}

// Load the user's preferred theme on page load (optional)
document.addEventListener('DOMContentLoaded', function () {
    loadThemePreference();
});







// Define the minimum and maximum font sizes
const MIN_FONT_SIZE = 12; // minimum font size in pixels
const MAX_FONT_SIZE = 18; // maximum font size in pixels

// Function to increase font size by 2px, with a maximum limit
function increaseFontSize() {
    const body = document.body;
    const computedStyle = window.getComputedStyle(body);
    let currentSize = parseFloat(computedStyle.getPropertyValue('font-size'));

    // Check if currentSize is less than MAX_FONT_SIZE
    if (currentSize < MAX_FONT_SIZE) {
        body.style.fontSize = Math.min(currentSize + 2, MAX_FONT_SIZE) + 'px';
    }
}

// Function to decrease font size by 2px, with a minimum limit
function decreaseFontSize() {
    const body = document.body;
    const computedStyle = window.getComputedStyle(body);
    let currentSize = parseFloat(computedStyle.getPropertyValue('font-size'));

    // Check if currentSize is greater than MIN_FONT_SIZE
    if (currentSize > MIN_FONT_SIZE) {
        body.style.fontSize = Math.max(currentSize - 2, MIN_FONT_SIZE) + 'px';
    }
}

// Function to reset font size to default (16px)
function resetFontSize() {
    document.body.style.fontSize = '16px';
}


const navLinks = document.querySelectorAll('.scrollspyTarget')
navLinks.forEach(navLink => {
    navLink.addEventListener('click', () => {
        navLinks.forEach(navLink => {
            navLink.classList.remove('active');
        });
        navLink.classList.add('active');
        Hash = navLink.attributes.href.value;
        tabId = Hash.slice(1);
        console.log(tabId);
        smoothScroll(tabId, 50)
    });
});

function smoothScroll(target, duration) {
    var target = document.getElementById(target)
    var targetPosition = target.getBoundingClientRect().top;
    var tp = targetPosition - 235;
    console.log('targetPosition = ' + tp);
    var startPostion = window.pageYOffset;
    var distance = tp;
    var startTime = null;

    function animation(currentTime) {
        console.log('startTime' + startTime);
        if (startTime === null) startTime = currentTime;
        var timeElapsed = currentTime - startTime;
        var run = ease(timeElapsed, startPostion, distance, duration);
        window.scrollTo(0, run);
        if (timeElapsed < duration) requestAnimationFrame(animation)
    }
    function ease(t, b, c, d) {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t + b;
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b
    }
    requestAnimationFrame(animation);

}

// ChatBot

$(document).ready(function () {
    $(".chatbot").click(function () {
        $(".chatbot").toggleClass("close");
        $(".chatbot-icons").slideToggle("slow");
    });
});
$(document).ready(function () {
    $("body").tooltip({ selector: '[data-toggle=tooltip]' });
});



$(".carouselPlacementsLaw").owlCarousel({
    margin: 20,
    loop: true,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 2,
            nav: false
        },
        600: {
            items: 3,
            nav: false
        },
        1000: {
            items: 4,
            nav: false
        }
    }
});