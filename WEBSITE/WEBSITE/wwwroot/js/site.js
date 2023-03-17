var swiper = new Swiper(".mySwiper", {
    slidesPerView: 3,
    spaceBetween: 30,
    freeMode: true,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
});

const progressCircle = document.querySelector(".autoplay-progress svg");
const progressContent = document.querySelector(".autoplay-progress span");
var swiper = new Swiper(".store-Swiper-1", {
    spaceBetween: 30,
    centeredSlides: true,
    autoplay: {
        delay: 2500,
        disableOnInteraction: false
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true
    },
    navigation: {
        nextEl: ".swiper-button-next-1",
        prevEl: ".swiper-button-prev-1"
    },
    on: {
        autoplayTimeLeft(s, time, progress) {
            progressCircle.style.setProperty("--progress", 1 - progress);
            progressContent.textContent = `${Math.ceil(time / 1000)}s`;
        }
    }
});

$(document).ready(function () {
    footer_menu_click("store");
})
function footer_menu_click(name) {
    document.getElementById("home").style.display = 'none';
    document.getElementById("store").style.display = 'none';
    document.getElementById("clipboard").style.display = 'none';
    document.getElementById("location").style.display = 'none';
    document.getElementById("menu").style.display = 'none';
    document.getElementById("user").style.display = 'none';
    document.getElementById(name).style.display = 'block';

}
function user_click() {
    footer_menu_click("user");
}
function collapse_onlick(id) {
    if (document.getElementById(id).style.display == "") {
        document.getElementById(id).style.display = "none"
    } else {
        document.getElementById(id).style.display = ""
    }

}
function login(input) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/home/login",
        data: { AppName: input },
        success: function (data) {
            debugger
            console.log(data);
        }
    });
}

    //---------------------------------------------------------------------------------------------//

