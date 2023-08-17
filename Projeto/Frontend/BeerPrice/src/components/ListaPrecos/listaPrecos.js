import React from "react";
import { View } from "react-native";
import { Container } from "./stylePrecos";
import { List } from "react-native-paper";
import fonts from "../../styles/fonts";
import { Linking } from "react-native";
import { Text, TouchableRipple } from "react-native-paper";

function ListaPrecos({ data }) {
  return (
    <Container>
      <View style={{ marginBottom: 10 }}>
        <List.Item
          title={data.mercado}
          description={`R$  ${data.preco}`}
          titleStyle={{ color: "#FFF", fontSize: 16 }} // Defina a cor para o título
          descriptionStyle={{
            color: "#fff",
            fontFamily: fonts.fonts.outros,
            fontSize: 13,
          }} // Defina a cor para a descrição
          left={(props) => (
            <TouchableRipple onPress={() => Linking.openURL(data.ecommerce)} activeOpacity={2} >
              <List.Icon {...props} icon="cart" color="#FFF"/>
            </TouchableRipple>
          )}
        />
      </View>
    </Container>
  );
}

export default ListaPrecos;
