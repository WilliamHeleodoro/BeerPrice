import React, { useEffect } from "react";
import Routes from "./src/routes/rotas";

import {
  useFonts,
  Montserrat_700Bold,
  Montserrat_300Light_Italic,
  Montserrat_600SemiBold,
} from "@expo-google-fonts/montserrat";
import * as SplashScreen from "expo-splash-screen";

import Toast from "react-native-toast-message";

function App() {
  const [fontsLoaded] = useFonts({
    Montserrat_700Bold,
    Montserrat_300Light_Italic,
    Montserrat_600SemiBold,
  });
  useEffect(() => {
    async function prepare() {
      await SplashScreen.preventAutoHideAsync();

      if (fontsLoaded) {
        SplashScreen.hideAsync();
      }
    }

    prepare();
  }, [fontsLoaded]);

  if (!fontsLoaded) {
    return null;
  }

  return (
    <>
      <Routes />
      <Toast />
    </>
  );
}

export default App;
