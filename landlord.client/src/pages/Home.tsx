import WeatherForecast from "../components/WeatherForecast.tsx";
import LogoutLink from "../components/LogoutLink.tsx";
import AuthorizeView, { AuthorizedUser } from "../components/AuthorizeView.tsx";

function Home() {
    return (
        <AuthorizeView>
            <span><LogoutLink> Logout <AuthorizedUser value="email" /></LogoutLink></span>
            <WeatherForecast/>
        </AuthorizeView>
    );
}

export default Home;