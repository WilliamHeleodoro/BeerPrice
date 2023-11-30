import {
  Container,
  SearchContainer,
  Input,
  SearchButton,
  ListaCervejas,
  Rodape,
  AvalieProduto,
} from "./styles";
import React, { useRef, useState, useEffect } from "react";
import { ActivityIndicator, Text } from "react-native";
import { Feather } from "@expo/vector-icons";
import { Linking, View } from "react-native";
import Header from "../../components/Header";
import Cervejas from "../../components/ListaCervejas/listaCervejas";
import { getCervejasPaginado } from "../../services/apiCervejas";
import { useNavigation } from "@react-navigation/native";
import Toast from "react-native-toast-message";

function Home() {
  const [pesquisa, setPesquisa] = useState("");
  const [naoTemPaginacao, setNaoTemPaginacao] = useState(false);
  const [loading, setLoading] = useState(true);
  const [cervejas, setCervejas] = useState([]);
  const [pagina, setPagina] = useState(1);
  const [refreshing, setRefreshing] = useState(false);
  const [refreshingPaginacao, setRefreshingPaginacao] = useState(false);
  const listaCervejasRef = useRef(null);

  const scrollToTop = () => {
    if (listaCervejasRef.current) {
      listaCervejasRef.current.scrollToOffset({ offset: 0, animated: true });
    }
  };

  const linhas = 10;

  const navigation = useNavigation();

  const fetchCervejas = (newPage = 1) => {
    setNaoTemPaginacao(false);
    console.log("página", newPage);
    getCervejasPaginado({
      pagina: newPage,
      linhas: linhas,
      pesquisa: pesquisa.trim(),
    })
      .then((response) => {
        const paginacaoResponse = response.data;

        if (pesquisa.trim() != "" && paginacaoResponse.dados.length === 0) {
          Toast.show({
            type: "info",
            text1: "Produto não encontrado",
          });
        }

        // VALIDA SE TEM MAIS ITENS

        if (paginacaoResponse.dados.length < linhas) {
          setNaoTemPaginacao(true);
        } else {
          setPagina(newPage + 1);
        }
        setCervejas((prevCervejas) =>
          newPage === 1
            ? paginacaoResponse.dados
            : [...prevCervejas, ...paginacaoResponse.dados]
        );
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
        setRefreshingPaginacao(false);
      });
  };

  const handleSearchButtonPress = () => {
    setPagina(1);
    fetchCervejas();
  };

  // Chamada inicial quando a página é carregada
  useEffect(() => {
    fetchCervejas();
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
          value={pesquisa}
          onChangeText={setPesquisa}
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
        ListFooterComponent={() =>
          refreshingPaginacao && (
            <View
              style={{
                padding: 10,
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <Text style={{ color: "#FFF" }}>Carregando...</Text>
            </View>
          )
        }
        onEndReached={() => {
          if (!naoTemPaginacao && !refreshingPaginacao) {
            setRefreshingPaginacao(true);

            fetchCervejas(pagina, true);
          }
        }}
        onEndReachedThreshold={0.5}
        renderItem={({ item }) => (
          <Cervejas
            data={item}
            navigatePage={() => navigateDetailsPage(item)}
          />
        )}
        keyExtractor={(item) => String(item.id)}
        refreshing={refreshing}
        onRefresh={() => fetchCervejas()}
      />
      <Rodape
        onPress={() => Linking.openURL("https://forms.gle/1rx4ibq9wtVtf9mt7")}
        activeOpacity={0.1}
      >
        <AvalieProduto>Avalie esse Aplicativo</AvalieProduto>
      </Rodape>
    </Container>
  );
}

export default Home;
