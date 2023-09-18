import React, { useRef, useState, useEffect } from "react";
import { ActivityIndicator } from "react-native";

import {
  Container,
  SearchContainer,
  Input,
  SearchButton,
  ListaCervejas,
  Rodape
} from "./styles";
import { Feather } from "@expo/vector-icons";

import Header from "../../components/Header";
import Cervejas from "../../components/ListaCervejas/listaCervejas";

import { getCervejas } from "../../services/apiCervejas";

import { useNavigation } from "@react-navigation/native";

import Toast from "react-native-toast-message";

import { Linking } from "react-native";

function Home() {
  const listaCervejasRef = useRef(null);
  const scrollToTop = () => {
    if (listaCervejasRef.current) {
      listaCervejasRef.current.scrollToOffset({ offset: 0, animated: true });
    }
  };

  const [resultadoFiltro, setResultadoFiltro] = useState("");
  const [loading, setLoading] = useState(true);
  const [cervejas, setCervejas] = useState([]);

  const navigation = useNavigation();

  const fetchCervejas = (filtro) => {
    setLoading(true);

    getCervejas({ filtroGeral: filtro })
      .then((response) => {
        setCervejas(response.data);
      })
      .catch((err) => {
        console.error("Ops! Ocorreu um erro:", err, err.response.data);
        Toast.show({
          type: "error",
          text1: err.response.data,
        });
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleSearchButtonPress = () => {
    fetchCervejas(resultadoFiltro.trim());
  };

  // Chamada inicial quando a página é carregada
  useEffect(() => {
    fetchCervejas(resultadoFiltro.trim());
  }, []);

  function navigateDetailsPage(item) {
    navigation.navigate("Detail", { id: item.id });
  }

  if (loading) {
    return (
      <Container>
        <ActivityIndicator size="large" color="#fff" />
      </Container>
    );
  }
  return (
    <Container>
      <Header title="Catálogo de Cervejas" scrollToTop={scrollToTop} />

      <SearchContainer>
        <Input
          placeholder="Busque por Marca, Tipo..."
          placeholderTextColor="#fff"
          value={resultadoFiltro}
          onChangeText={setResultadoFiltro}
        />
        <SearchButton onPress={handleSearchButtonPress}>
          <Feather name="search" size={30} color="#FFF" />
        </SearchButton>
      </SearchContainer>

      <ListaCervejas
        ref={listaCervejasRef}
        numColumns={2}
        showsVerticalScrollIndicator={false}
        data={cervejas}
        renderItem={({ item }) => (
          <Cervejas
            data={item}
            navigatePage={() => navigateDetailsPage(item)}
          />
        )}
        keyExtractor={(item) => String(item.id)}
      />
      <Rodape
      onPress={() => Linking.openURL("https://forms.gle/1rx4ibq9wtVtf9mt7")} activeOpacity={2}
      >
        Avalie esse Aplicativo
      </Rodape>
    </Container>
  );
}

export default Home;
