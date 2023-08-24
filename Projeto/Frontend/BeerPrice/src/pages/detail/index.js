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
  Grafico,
  DetalhesGrafico,
  Atualizado,
  ContainerAtualizacao,
} from "./styles";

import { ActivityIndicator } from "react-native";

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

import { Feather } from "react-native-vector-icons";

function Detail() {
  const navigation = useNavigation();
  const route = useRoute();

  const [loading, setLoading] = useState(true);
  const [itemCerveja, setItemCerveja] = useState([]);
  const [historicoPreco, setHistoricoPreco] = useState([]);
  const [selected, setSelected] = useState(0);

  useEffect(() => {
    if (route.params?.id) {
      setLoading(true);
      getCervejaDetalhes(route.params.id)
        .then((response) => {
          setItemCerveja(response.data);
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
            "ops! ocorreu um erro: " + err + " " + err.response.data
          );
        })
        .finally(() => {
          setLoading(false); // Move this inside the .finally block
        });
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
        <ContainerAtualizacao>
          <Feather
            name="clock"
            size={14}
            color={"#a9a9a9"}
            style={{ marginRight: 5 }}
          />
          <Atualizado>{itemCerveja.dataAtualizacao}</Atualizado>
        </ContainerAtualizacao>
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
      <Grafico>
        <DetalhesGrafico>
          <Feather
            name="bar-chart"
            size={30}
            color={"#fff"}
            marginTop={"1%"}
            marginLeft={"3%"}
          />
          <Text
            variant="labelLarge"
            style={{
              color: "#fff",
              fontSize: 18,
              marginTop: "3%",
              marginLeft: 15,
              fontWeight: "bold",
            }}
          >
            Histórico do item por Mercado
          </Text>
        </DetalhesGrafico>
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
          style={{ flex: 2 }}
          data={historicoPreco.map((item) => item.preco)}
          svg={{ stroke: "rgb(134, 65, 244)", strokeWidth: 1.8 }}
          contentInset={{ top: 40, bottom: 40, right: 30, left: 30 }}
          curve={shape.curveMonotoneX}
        >
          <Markers />
          <Labels />
        </LineChart>
      </Grafico>
    </Container>
  );
}

export default Detail;
