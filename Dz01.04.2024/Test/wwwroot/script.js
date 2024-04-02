$(function(){
    const icons = {
        "01d": "https://openweathermap.org/img/wn/01d.png",
        "01n": "https://openweathermap.org/img/wn/01n.png",
        "02d": "https://openweathermap.org/img/wn/02d.png",
        "02n": "https://openweathermap.org/img/wn/02n.png",
        "03d": "https://openweathermap.org/img/wn/03d.png",
        "03n": "https://openweathermap.org/img/wn/03n.png",
        "04d": "https://openweathermap.org/img/wn/04d.png",
        "04n": "https://openweathermap.org/img/wn/04n.png",
        "09d": "https://openweathermap.org/img/wn/09d.png",
        "09n": "https://openweathermap.org/img/wn/09n.png",
        "10d": "https://openweathermap.org/img/wn/10d.png",
        "10n": "https://openweathermap.org/img/wn/10n.png",
        "11d": "https://openweathermap.org/img/wn/11d.png",
        "11n": "https://openweathermap.org/img/wn/11n.png",
        "13d": "https://openweathermap.org/img/wn/13d.png",
        "13n": "https://openweathermap.org/img/wn/13n.png",
        "50d": "https://openweathermap.org/img/wn/50d.png",
        "50n": "https://openweathermap.org/img/wn/50n.png"
    };
    $('.but').click(function(){
        $('#maintext').text('Текущая погода!');
        $('#maintext').css('color','rgb(0, 97, 208)');
        const cityName = $('.inpcity').val();
        const apiKey = 'b84c2769ac55a507a64a5e529ec8f370'; // Заменить его!
        const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${cityName}&appid=${apiKey}&units=metric`;
        fetch(apiUrl).then(response => response.json()).then(data => {
            const currentWeather = data.weather[0], mainWeather = data.main, wind = data.wind;
            if (currentWeather && mainWeather && wind) {
                const formattedDate = new Date(data.dt * 1000).toLocaleDateString('ru-RU', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
                $('#cityName').text(cityName);
                $('#date').text(formattedDate);
                $('#weatherType').text(currentWeather.description);
                $('#temperature').text(`${Math.round(mainWeather.temp)}°C`);
                $('#minTemperature').text(`Мин температура: ${Math.round(mainWeather.temp_min)}°C`);
                $('#maxTemperature').text(`Макс температура: ${Math.round(mainWeather.temp_max)}°C`);
                $('#windSpeed').text(`Скорость ветра(км/ч): ${wind.speed}`);
                const iconUrl = icons[currentWeather.icon];
                if (iconUrl) $('#weatherImage').attr('src', iconUrl);
                const hourlyApiUrl = `https://api.openweathermap.org/data/2.5/forecast?q=${cityName}&appid=${apiKey}&units=metric`;
                fetch(hourlyApiUrl).then(response => response.json()).then(data => {
                        const hourlyWeather = data.list.slice(0, 12);
                        let hourlyHtml = '';
                        hourlyWeather.forEach(hour => {
                            const formattedHour = new Date(hour.dt * 1000).toLocaleTimeString('ru-RU', { hour: 'numeric', minute: 'numeric' });
                            hourlyHtml += `
                                <div class="wplate">
                                    <img class="timg" src="https://openweathermap.org/img/wn/${hour.weather[0].icon}.png">
                                    <p>${formattedHour}</p>
                                    <p>${hour.weather[0].description}</p>
                                    <p>${Math.round(hour.main.temp)}°C</p>
                                    <p>${hour.wind.speed} км/ч</p>
                                </div>
                            `;
                        });
                        
                        $('#hourlyWeather').html(hourlyHtml);
                    })
                    .catch(error => {
                        console.error('Ошибка запроса почасовой погоды:', error);
                    });
            } else {
                console.error('Ошибка запроса: Некорректные данные получены!');
                $('#maintext').text('Ошибка запроса: Некорректные данные получены!');
                $('#maintext').css('color', 'red');
            }
        })
        .catch(error => {
            console.error('Ошибка запроса:', error);
            $('#maintext').text('Ошибка запроса!');
            $('#maintext').css('color', 'red');
        });
    });
});