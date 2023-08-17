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

import ListaPrecos from "../../components/ListaPrecos/listaPrecos";

import DetalhesCerveja from "../../components/DetalhesCerveja/detalhesCerveja";

import { LineChart } from "react-native-svg-charts";
import * as shape from "d3-shape";

import {
  Markers,
  Labels,
} from "../../components/ComponentesGraficos/ChartComponents";

import FiltroGrafico from "../../components/FiltroGrafico/filtroGrafico";

import {
  getCervejaDetalhes,
  getHistoricoPreco,
} from "../../services/apiCervejas";

import { Text } from "react-native-paper";

function Detail() {
  const navigation = useNavigation();
  const route = useRoute();

  const [loading, setLoading] = useState(true);
  const [itemCerveja, setItemCerveja] = useState([]);
  const [historicoPreco, setHistoricoPreco] = useState([]);
  const [selected, setSelected] = useState(0);

  useEffect(() => {
    if (route.params?.id) {
      getCervejaDetalhes(route.params.id)
        .then((response) => {
          setItemCerveja(response.data);
          setLoading(false);
        })
        .catch((err) => {
          console.error(
            "ops! ocorreu um erro : " + err + " " + err.response.data
          );
        });
      setLoading(false);
    }
  }, []);

  const itens = itemCerveja.maiorPreco || [];
  const mercados = itens.map((item) => item.mercado);
  const nomeMercado = mercados[selected];
  const [nomeDoMercadoSelecionado, setNomeDoMercadoSelecionado] = useState("");

  useEffect(() => {
    setNomeDoMercadoSelecionado(nomeMercado);
  });
  
  useEffect(() => {
    if (nomeDoMercadoSelecionado !== "") {
      getHistoricoPreco(route.params?.id, nomeDoMercadoSelecionado)
        .then((response) => {
          setHistoricoPreco(response.data);
          setLoading(false);
        })
        .catch((err) => {
          console.error(
            "ops! ocorreu um erro : " + err + " " + err.response.data
          );
        });
      setLoading(false);
    }
  }, [nomeDoMercadoSelecionado]);

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
      <Text
        variant="labelLarge"
        style={{
          color: "#fff",
          fontSize: 18,
          marginTop: 12,
          marginLeft: 15,
          fontWeight: 'bold'
        }}
      >
        Histórico do item por Mercado
      </Text>
      <FiltroGrafico
        selected={selected}
        options={mercados}
        horizontal={true}
        onChangeSelect={(opt, i) => {
          setSelected(i);
          setNomeDoMercadoSelecionado(mercados[i]);
        }}
      />

      <LineChart
        style={{ height: 200 }}
        data={historicoPreco.map((item) => item.preco)}
        svg={{ stroke: "rgb(134, 65, 244)", strokeWidth: 1.8 }}
        contentInset={{ top: 40, bottom: 40, right: 25, left: 25 }}
        curve={shape.curveMonotoneX}
      >
        <Markers />
        <Labels />
      </LineChart>
    </Container>
  );
}

export default Detail;
