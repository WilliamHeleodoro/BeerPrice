import React from "react";
import { ScrollView } from "react-native";

import { Container, SearchContainer, Input, SearchButton, ListaCervejas } from "./styles";
import { Feather } from "@expo/vector-icons";

import Header from "../../components/Header";
import Cervejas from "../../components/ListaCervejas/listaCervejas";

function Home() {
  return (
    <Container>
      <Header title="Cátalogo de Cervejas" />

      <SearchContainer>
        <Input placeholder="Marca da Cerveja" placeholderTextColor="#ddd" />
        <SearchButton>
          <Feather name="search" size={30} color="#FFF" />
        </SearchButton>
      </SearchContainer>
      <ScrollView>
        <ListaCervejas
            data={[1,2,3,4]}
            renderItem={ ({ item }) => <Cervejas/>}
        />
      </ScrollView>
    </Container>
  );
}

export default Home;
