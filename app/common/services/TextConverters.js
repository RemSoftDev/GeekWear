window.GeekWear.app.factory('TextConverters', function () {
    var transforms = {
        notransform: {
            type: 'notransform',
            label: 'Plain text (no transform)',
            transformer: function (text) {
                return text;
            }
        },
        binary: {
            type: 'binary',
            label: 'Text to Binary',
            transformer: function (text) {
                //thx to: http://stackoverflow.com/a/14430733
                var x = '';
                for (i = 0; i < text.length; i++) {
                    var a = text[i].charCodeAt(0).toString(2);
                    a = new Array(9 - a.length).join('0') + a;

                    x += a + ' ';
                }
                return x;
            }
        },
        hexadecimal: {
            type: 'hexadecimal',
            label: 'Text to Hexadecimal',
            transformer: function (text) {
                //thx to http://stackoverflow.com/a/21648161
                var hex, i;
                var result = '';
                for (i = 0; i < text.length; i++) {
                    hex = text.charCodeAt(i).toString(16);
                    result += (' ' + hex).slice(-4);
                }

                return result.trim();
            }
        },
        octal: {
            type: 'octal',
            label: 'Text to Octal',
            transformer: function (text) {
                return encode(text);

                //thx to http://stackoverflow.com/a/30237853
                function encode(str) {
                    return decToOctBytes(charsToBytes(str.split(''))).join(' ');
                }

                function charsToBytes(chars) {
                    return chars.map(function (char) {
                        return char.charCodeAt(0);
                    });
                }

                function decToOctBytes(decBytes) {
                    return decBytes.map(function (dec) {
                        return ('000' + dec.toString(8)).substr(-3);
                    });
                }
            }
        },
        morse: {
            type: 'morse',
            label: 'Text to Morse',
            transformer: function (text) {
                if (morsejs) {
                    return morsejs.encode(text);
                }
            }
        },
        md5: {
            type: 'md5',
            label: 'Text to MD5',
            transformer: function (text) {
                if (md5) {
                    return md5(text);
                }
            }
        },
        html: {
            type: 'html',
            label: 'Text to HTML',
            transformer: function (text) {
                text = text.replace(/\r\n/g, '_5454df__rtxx').replace(/[\r\n]/g, '_5454df__rtxx');

                if (text.indexOf('_5454df__rtxx')) {
                    var arr = text.split('_5454df__rtxx');
                    text = '<p>' + htmlEscape('<p>') + arr.join(htmlEscape('</p>') + '</p>' + '<p>' + htmlEscape('<p>')) + htmlEscape('</p>') + '</p>';
                } else {
                    text = htmlEscape('<p>') + text + htmlEscape('</p>');
                }

                return text;

                //thx to http://stackoverflow.com/a/7124052
                function htmlEscape(str) {
                    return String(str)
                            .replace(/&/g, '&amp;')
                            .replace(/"/g, '&quot;')
                            .replace(/'/g, '&#39;')
                            .replace(/</g, '&lt;')
                            .replace(/>/g, '&gt;');
                }
            }
        },
    };

    return transforms;
});
