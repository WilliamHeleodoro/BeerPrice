import React from 'react';
import { Circle,Text } from 'react-native-svg';


export const Markers = ({ x, y, data }) => {
    return data.map((item, index) => (
        <Circle
            key={index}
            cx={x(index)}
            cy={y(item)}
            r={5}
            stroke="#fff"
            fill="#fff"
        />
    ));
};


export const Labels = ({ x, y, data }) => {
    return data.map((item, index) => (
        <Text
            key={index}
            x={x(index)}
            y={y(item) - 13}
            fontSize={12}
            fontWeight="lighter"
            stroke="#fff"
            fill="#fff"
            textAnchor="middle"
        >{`R$ ${item}`}</Text>
    ));
};