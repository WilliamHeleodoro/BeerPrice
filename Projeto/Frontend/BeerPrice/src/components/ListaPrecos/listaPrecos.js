import React from "react";
import { View, TouchableOpacity} from "react-native";
import { Container, CestaMercado } from "./stylePrecos";
import { List } from "react-native-paper";
import fonts from "../../styles/fonts";
import { Linking } from "react-native";

function ListaPrecos({ data }) {
  return (
    <Container>
      <View style={{ marginBottom: 10  }}>
        <List.Item
          title={data.mercado}
          description={`R$  ${data.preco}`}
          titleStyle={{ color: "#FFF",  fontFamily: fonts.fonts.outros, fontSize: 14, paddingLeft: "1%"}} // Defina a cor para o título
          descriptionStyle={{
            color: "#fff",
            fontFamily: fonts.fonts.outros,
            fontSize: 13,
            paddingLeft: "1%"
          }} // Defina a cor para a descrição
          left={(props) => (
            <TouchableOpacity onPress={() => Linking.openURL(data.ecommerce)} activeOpacity={0.5}>
              <List.Icon {...props} icon="cart" color="#FFF" style={{ marginTop: 'auto', marginBottom: 'auto', paddingLeft: "15%" }} />
            </TouchableOpacity>
          )}
          
        />
      </View>
    </Container>
  );
}

export default ListaPrecos;
