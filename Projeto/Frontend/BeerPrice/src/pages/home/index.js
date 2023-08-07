import React, { useState, useEffect } from "react";
import { ScrollView, ActivityIndicator } from "react-native";

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

import api from "../../services/api";

function Home() {
  const filtro = {
    filtroCaracteristica: "",
    filtroTipo: "",
    filtroUnidade: 0,
    filtroMarca: "",
    filtroQuantidade: "",
  };

  const [loading, setLoading] = useState(true);
  const [cervejas, setCervejas] = useState([]);
  useEffect(() => {
    const ac = new AbortController();

    api
      .post("/cervejas", filtro)
      .then((response) => {
        setCervejas(response.data);
      })
      .catch((err) => {
        console.error(
          "ops! ocorreu um erro : " + err + " " + err.response.data
        );
        ac.abort();
      });

    setLoading(false);
  }, []);

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
        <Input placeholder="Marca da Cerveja" placeholderTextColor="#ddd" />
        <SearchButton>
          <Feather name="search" size={30} color="#FFF" />
        </SearchButton>
      </SearchContainer>
      <ScrollView showsVerticalScrollIndicator={false}>
        <ListaCervejas
          numColumns={2}
          showsVerticalScrollIndicator={false}
          data={cervejas}
          renderItem={({ item }) => <Cervejas data={item} />}
          keyExtractor={(item) => String(item.id)}
        />
      </ScrollView>
    </Container>
  );
}

export default Home;
