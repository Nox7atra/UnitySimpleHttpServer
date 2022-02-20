# Unity Simple Http Server | Windows

Пример простого Http сервера и веб сервера встроенного в Unity. Бывает полезно для различных администрируемых проектов, стендов и т.п.
Плюс в качестве примера сделан веб геймпад для простенькой игры

Установка через Unity Package Manager / Add package from git URL:  https://github.com/Nox7atra/UnitySimpleHttpServer.git?path=/Assets/UnityHttpServer

<h3>Класс WebServer</h3>
Необходим для хостинга веб приложения внутри Unity приложения.
Корневым каталогом является StreamingAssets.
Относительный путь указывается в __Server Path__. Порт указывается в __Server Port__

<h3>Класс HTTPServer</h3>
Необходим для работы веб апи внутри Unity приложения.
Порт указывается в __Server Port__

<h3>Класс HTTPServerHandler</h3>

Для расширения функциональности необходимо создавать наследников класса HTTPServerHandler. Наследники класса через рефлексию будут собраны в HTTPServer и будут автоматически обрабатываться.

Расширение ProcessParams - позволяет обрабатывать параметры запроса и добавлять функциональность (даже если запрос без параметров. Параметры пишутся через / как часть пути)
Расширение GetAnswerData - позволяет писать свои сетевые ошибки, коды и возвращать данные о том, почему что-то пошло не так.

<h3>Пример:</h3>

    public class GamepadHandler : HTTPServerHandler
    {
        protected override string _route => "/gamepad/";
        private bool IsSuccess = true;
        private string ErrorMessage;
        public override void ProcessParams(string url)
        {
            base.ProcessParams(url);
            if(Enum.TryParse<KeyCode>(_params[1], true, out var key))
            {
                if (_params[0] == "down")
                {
                    WebInput.SetKeyDown(key);
                }
                else
                {

                    WebInput.SetKeyUp(key);
                }
            }
            else
            {
                ErrorMessage = $"Invalid keycode in param 1. There are no keycode {_params[0]}";
            }
        }
        public override NetworkAnswer GetAnswerData()
        {
            return new NetworkAnswer()
            {
                status = IsSuccess ? 200 : 500,
                errorMessage = IsSuccess ? null : ErrorMessage
            };
        }
    }

В данном примере при настроенном __ServerPort__ в классе HTTPServer. Для примера скажем порт 10021.
Урл по которому вызывется апи будет выглядеть как http://localhost:10021/down/A или http://{YourPCIP}:10021/down/A. Где _param[0] = down, param[1] = "A".<br/>
Для того, чтобы узнать айпи своего компьютера достаточно прописать в cmd ipconfig.
<h3>Важно!</h3>
Для работы в редакторе посмотрите, чтобы для него в фаерволле редактор Unity мог принимать Public соединения

<h3>Не сделано:</h3>
<ul>
<li>Разделение на методы GET, POST и т.п.</li>
<li>Настройка политики CORS</li>
</ul>
