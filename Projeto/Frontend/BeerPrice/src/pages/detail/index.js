import React, { useState, useEffect } from "react";
import {
  Container,
  Header,
  HeaderButton,
  Banner,
  Title,
  View,
  Precos,
  InfoProduto,
} from "./styles";

import { ActivityIndicator } from "react-native";

import { Feather } from "@expo/vector-icons";

import { useNavigation, useRoute } from "@react-navigation/native";
import api from "../../services/api";

import ListaPrecos from "../../components/ListaPrecos/listaPrecos";

import DetalhesCerveja from "../../components/DetalhesCerveja/detalhesCerveja";

function Detail() {
  const navigation = useNavigation();
  const route = useRoute();

  const [loading, setLoading] = useState(true);
  const [itemCerveja, setItemCerveja] = useState([]);

  useEffect(() => {
    const ac = new AbortController();

    api
      .get(`/cervejas/${route.params?.id}`)
      .then((response) => {
        setItemCerveja(response.data);
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
      <Header>
        <HeaderButton activeOpacity={0.7} onPress={() => navigation.goBack()}>
          <Feather name="arrow-left" size={28} color="#fff" />
        </HeaderButton>
        <Title>Cerveja</Title>
      </Header>
      <View>
        <Banner resizeMethod="resize" source={{ uri: itemCerveja.imagem }} />
        <Precos
          showsVerticalScrollIndicator={false}
          data={itemCerveja.maiorPreco}
          renderItem={({ item }) => <ListaPrecos data={item} />}
        />
      </View>
      <InfoProduto>
        <DetalhesCerveja
          texto={itemCerveja.titulo}
          quantidade={itemCerveja.quantidade}
          unidade={itemCerveja.unidade}
        />
      </InfoProduto>
    </Container>
  );
}

export default Detail;
