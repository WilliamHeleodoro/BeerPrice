import React, { useState, useEffect } from "react";
import { ActivityIndicator } from "react-native";

import {
  Container,
  SearchContainer,
  Input,
  SearchButton,
  ListaCervejas,
} from "./styles";
import { Feather } from "@expo/vector-icons";

import Header from "../../components/Header";
import Cervejas from "../../components/ListaCervejas/listaCervejas";

import { getCervejas } from "../../services/apiCervejas";

import { useNavigation } from "@react-navigation/native";

import  Toast  from "react-native-toast-message";

function Home() {
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
      .catch((err )=> {
        console.error("Ops! Ocorreu um erro:", err, err.response.data);
        Toast.show({
          type: 'error',
          text1: err.response.data
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
      <Header title="Cátalogo de Cervejas" />

      <SearchContainer>
        <Input
          placeholder="Marca da Cerveja"
          placeholderTextColor="#fff"
          value={resultadoFiltro}
          onChangeText={setResultadoFiltro}
        />
        <SearchButton onPress={handleSearchButtonPress}>
          <Feather name="search" size={30} color="#FFF" />
        </SearchButton>
      </SearchContainer>

      <ListaCervejas
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
    </Container>
  );
}

export default Home;
