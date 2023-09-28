import React from "react";

import { Container, BannerItem, Title, Detalhes } from "./styleCervejas";
import { View } from "react-native";

function ListaCervejas({ data, navigatePage }) {
  return (
    <Container activeOpacity={0.7} onPress={() => navigatePage(data)}>
      <BannerItem
        resizeMethod="resize"
        source={{
          uri: data.imagem,
        }}
      />
      <Detalhes data={data}>
        <Title numberOfLines={1}>{data.titulo}</Title>
        <Title>
          {data.quantidade} Unidade: {data.unidade}
        </Title>
        <Title>R$ {data.preco}</Title>
      </Detalhes>
    </Container>
  );
}

export default ListaCervejas;
