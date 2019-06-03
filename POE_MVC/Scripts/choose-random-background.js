var index = Math.ceil(Math.random() * 3)-1;
var images = ['cloudy.jpg', 'skyWithClouds.jpg', 'sunny.png'];
var path = '../Content/Images/' + images[index];
console.log(path);

$('body').css('background-image', 'url(' + path + ')');