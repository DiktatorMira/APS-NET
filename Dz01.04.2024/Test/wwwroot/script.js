$(function(){
    $('.but').click(function(){
        const cityName = $('.inpcity').val();
        const apiKey = 'b84c2769ac55a507a64a5e529ec8f370';
        const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${cityName}&dt={time}&appid=${apiKey}`;
        $('#maintext').text('Текущая погода!');
        $('#maintext').css('color','rgb(0, 97, 208)');
        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                const lat = data.coord.lat;
                const lon = data.coord.lon;
                const currentTime = Math.floor(Date.now() / 1000); // Текущее время в формате Unix time
                const apiUrl = `https://api.openweathermap.org/data/3.0/onecall/timemachine?lat=${lat}&lon=${lon}&dt=${currentTime}&appid=${apiKey}`;

                fetch(apiUrl)
                    .then(response => response.json())
                    .then(data => {
                        const currentWeather = data.current;
                        const dailyWeather = data.daily;

                        // Обработка текущей погоды
                        const currentDate = new Date(currentWeather.dt * 1000);
                        const formattedDate = currentDate.toLocaleDateString('ru-RU', {weekday: 'long', year: 'numeric', month: 'long', day: 'numeric'});
                        $('#cityName').text(cityName);
                        $('#date').text(formattedDate);
                        $('#weatherType').text(currentWeather.weather[0].description);
                        $('#temperature').text(`${Math.round(currentWeather.temp - 273.15)}°C`);
                        $('#minTemperature').text(`Мин температура: ${Math.round(dailyWeather[0].temp.min - 273.15)}°C`);
                        $('#maxTemperature').text(`Макс температура: ${Math.round(dailyWeather[0].temp.max - 273.15)}°C`);
                        $('#windSpeed').text(`Скорость ветра(км/ч): ${currentWeather.wind_speed}`);

                        // Обработка почасовой погоды
                        const hourlyWeather = data.hourly.slice(0, 12);
                        let hourlyHtml = '';
                        hourlyWeather.forEach(hour => {
                            const date = new Date(hour.dt * 1000);
                            const formattedHour = date.toLocaleTimeString('ru-RU', {hour: 'numeric', minute: 'numeric'});
                            const formattedDay = date.toLocaleDateString('ru-RU', {weekday: 'long'});
                            hourlyHtml += `
                                <div class="wplate">
                                    <img class="timg" src="http://openweathermap.org/img/wn/${hour.weather[0].icon}.png">
                                    <p>${formattedHour}</p>
                                    <p>${formattedDay}</p>
                                    <p>${Math.round(hour.temp - 273.15)}°C</p>
                                    <p>${hour.wind_speed} км/ч</p>
                                </div>
                            `;
                        });
                        $('#hourlyWeather').html(hourlyHtml);
                    })
                    .catch(error => {
                        console.error('Ошибка запроса:', error);
                        $('#maintext').text('Ошибка запроса!');
                        $('#maintext').css('color','red');
                    });
            })
            .catch(error => {
                console.error('Ошибка запроса:', error);
                $('#maintext').text('Ошибка запроса!');
                $('#maintext').css('color','red');
            });
    });
});